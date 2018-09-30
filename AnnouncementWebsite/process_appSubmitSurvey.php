<?php
require_once("dbcontroller.php");
$errorPresence = false;
$errorMessage = "";
$username = "";
$survey = $surveyID = "";
$text_answer = "";
//Validates that all the required parameters are valid and not empty
if(empty($_POST["username"])) {
    $errorPresence = true;
    $errorMessage = "Username must not be empty. ";
} else {
    $username = sanitizeInput($_POST["username"]);
    try {
        //Validate the surveyJson is not empty
        if(empty($_POST["surveyJson"])) {
            $errorPresence = true;
            $errorMessage = "Missing survey answers. Please try again!";
        } else {
            //Decode survey JSON object and get the survey information
            $jsonString = $_POST["surveyJson"];
            $survey = json_decode($jsonString);

            //Validate that the survey exist in database
            if ($survey->surveyID != null && $survey->surveyID != "") {
                $surveyID = sanitizeInput($survey->surveyID);
                $db_handle = new DBController();
                $selectQuery = $db_handle->getConn()->prepare("SELECT surveyID FROM survey WHERE surveyID = :surveyID");
                $selectQuery->bindParam(":surveyID", $surveyID);
                $selectQuery->execute();
                $selectResult = $selectQuery->fetchAll();
                $total = count($selectResult);

                if($total == 0) {
                    $errorPresence = true;
                    $errorMessage = "Survey does not exist. Please try again!";
                } else {
                    //Validate if user has participated before
                    $selectQuery = $db_handle->getConn()->prepare("SELECT userID FROM survey_response WHERE surveyID = :surveyID");
                    $selectQuery->bindParam(":surveyID", $surveyID);
                    $selectQuery->execute();
                    $selectResult = $selectQuery->fetchAll();
                    $total = count($selectResult);
                    
                    if ($total > 0) {
                        $errorPresence = true;
                        $errorMessage = "You have already participated before. Thank you for your participation!";
                    } else {
                        //Validate the existence of each question in database
                        for($i = 0; $i < count($survey->surveyQuestions); $i++){
                            $questionID = sanitizeInput($survey->surveyQuestions[$i]->questionID);

                            $selectQuery = $db_handle->getConn()->prepare("SELECT questionID FROM survey_question WHERE questionID = :questionID");
                            $selectQuery->bindParam(":questionID", $questionID);
                            $selectQuery->execute();
                            $selectResult = $selectQuery->fetchAll();
                            $totalResult = count($selectResult);

                            if ($totalResult == 0) {
                                $errorPresence = true;
                                $errorMessage = "Survey question does not exist. Please try again!";
                                break;
                            } else {
                                //Validate that the selected option of the question exist in database
                                if ($survey->surveyQuestions[$i]->surveyOptions != null) {
                                    if ($survey->surveyQuestions[$i]->questionType == "checkboxes") {
                                        $answers = explode(',', $survey->surveyQuestions[$i]->responseAnswer);

                                        foreach ($answers as $answerID) {
                                            $answerID = sanitizeInput($answerID);
                                            $selectQuery = $db_handle->getConn()->prepare("SELECT answerID FROM survey_questionanswer WHERE answerID = :answerID");
                                            $selectQuery->bindParam(":answerID", $answerID);
                                            $selectQuery->execute();
                                            $selectResult = $selectQuery->fetchAll();
                                            $totalResult = count($selectResult);

                                            if ($totalResult == 0) {
                                                $errorPresence = true;
                                                $errorMessage = "Survey option does not exist. Please try again! " . $answerID;
                                                break;
                                            }
                                        }
                                    } else {
                                        $answerID = sanitizeInput($survey->surveyQuestions[$i]->responseAnswer);

                                        $selectQuery = $db_handle->getConn()->prepare("SELECT answerID FROM survey_questionanswer WHERE answerID = :answerID");
                                        $selectQuery->bindParam(":answerID", $answerID);
                                        $selectQuery->execute();
                                        $selectResult = $selectQuery->fetchAll();
                                        $totalResult = count($selectResult);

                                        if ($totalResult == 0) {
                                            $errorPresence = true;
                                            $errorMessage = "Survey option does not exist. Please try again! " . $answerID;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                $errorMessage = "Survey ID must not be empty. Please try again!";
            }
            //echo $survey->surveyQuestions[0]->questionID;
        }
    } catch(PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
    
}

if (!$errorPresence) {
    //try {
        //Insert survey response into database
        //Generate unique ID
        $digits = 7;
        $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
        $isIDUnique = false;

        //Check for ID uniqueness  
        do {
           $selectQuery = $db_handle->getConn()->prepare("SELECT responseID FROM survey_response WHERE responseID = :responseID");
           $selectQuery->bindParam(":responseID", $randomID);
           $selectQuery->execute();
           $selectResult = $selectQuery->fetchAll();
           $total = count($selectResult);

           if($total == 0) {
               $isIDUnique = true;
           } else {
               $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
           }
        } while(!$isIDUnique);  
        $responseID = $randomID;
        //Insert information into database
        $insertResponseQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response (responseID, userID, surveyID, dateSubmitted) VALUES
        (:responseID, :userID, :surveyID, NOW())");

        $insertResponseQuery->bindParam(":responseID", $responseID);
        $insertResponseQuery->bindParam(":userID", $username);
        $insertResponseQuery->bindParam(":surveyID", $surveyID);
        $insertResult = $insertResponseQuery->execute();

        if($insertResult) {
            //Insert the survey answers into the database
            for($i = 0; $i < count($survey->surveyQuestions); $i++){
                $question = $survey->surveyQuestions[$i];
                $questionID = sanitizeInput($question->questionID);

                //Generate unique ID for answer
                $digits = 7;
                $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                $isIDUnique = false;

                //Check for ID uniqueness  
                do {
                   $selectQuery = $db_handle->getConn()->prepare("SELECT resp_answerID FROM survey_response_answer WHERE resp_answerID = :resp_answerID");
                   $selectQuery->bindParam(":resp_answerID", $randomID);
                   $selectQuery->execute();
                   $selectResult = $selectQuery->fetchAll();
                   $total = count($selectResult);

                   if($total == 0) {
                       $isIDUnique = true;
                   } else {
                       $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                   }
                } while(!$isIDUnique);

                $resp_answerID = $randomID;

                if ($question->questionType == "longsentece" || $question->questionType == "shortsentence") {
                    $answerID = "";
                    $text_answer = sanitizeInput($survey->surveyQuestions[$i]->responseAnswer);

                    $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                    $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                    $insertQuery->bindParam(":responseID", $responseID);
                    $insertQuery->bindParam(":questionID", $questionID);
                    $insertQuery->bindParam(":answerID", $answerID);
                    $insertQuery->bindParam(":text_answer", $text_answer);
                    
                    $insertResult = $insertQuery->execute();

                    if (!$insertResult) {
                        $errorPresence = true;
                        $errorMessage = "Failed to submit answer.";
                    }
                } else {
                    //Insert answer into database
                    if ($survey->surveyQuestions[$i]->questionType == "checkboxes") {
                        $answers = explode(',', $survey->surveyQuestions[$i]->responseAnswer);
                        $text_answer = "";
                        //Insert all the checked option into database
                        foreach ($answers as $answerID) {
                            $answerID = sanitizeInput($answerID);

                            $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                            $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                            $insertQuery->bindParam(":responseID", $responseID);
                            $insertQuery->bindParam(":questionID", $questionID);
                            $insertQuery->bindParam(":answerID", $answerID);
                            $insertQuery->bindParam(":text_answer", $text_answer);
                            
                            $insertResult = $insertQuery->execute();

                            if (!$insertResult) {
                                $errorPresence = true;
                                $errorMessage = "Failed to submit answer.";
                            }

                            //Check for ID uniqueness  
                            do {
                               $selectQuery = $db_handle->getConn()->prepare("SELECT resp_answerID FROM survey_response_answer WHERE resp_answerID = :resp_answerID");
                               $selectQuery->bindParam(":resp_answerID", $randomID);
                               $selectQuery->execute();
                               $selectResult = $selectQuery->fetchAll();
                               $total = count($selectResult);

                               if($total == 0) {
                                   $isIDUnique = true;
                               } else {
                                   $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                               }
                            } while(!$isIDUnique);

                            $resp_answerID = $randomID;
                        }
                    } else {
                        $answerID = sanitizeInput($survey->surveyQuestions[$i]->responseAnswer);
                        $text_answer = "";
                        $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                        $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                        $insertQuery->bindParam(":responseID", $responseID);
                        $insertQuery->bindParam(":questionID", $questionID);
                        $insertQuery->bindParam(":answerID", $answerID);
                        $insertQuery->bindParam(":text_answer", $text_answer);
                        
                        $insertResult = $insertQuery->execute();

                        if (!$insertResult) {
                            $errorPresence = true;
                            $errorMessage = "Failed to submit answer.";
                        }
                    }
                }      
            }
            if (!$errorPresence) {
                echo "Successfully submitted survey. Thank you for participating!";
            } else {
                echo "Error occured while submitting survey. Please try again!";
            }
        } else {
            echo "Failed to submit survey. Please try again!";
        }
    /*} catch(PDOException $e) {
        echo "Insert Error: " . $e->getMessage();
    }*/
    
} else {
    echo $errorMessage;
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>