<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

$surveyID="";

/* Retrieving Survey Questions' details */
if(isset($_POST["surveyidinput"])) {
    $surveyID = sanitizeInput($_POST["surveyidinput"]);
    //Obtain survey question details
    $query = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID ORDER BY questionNumber ASC");
    $query->bindParam(":surveyID", $surveyID);
    $query->execute();
    $result = $query->fetchAll();  
} else {
    $error_message = "No survey ID provided";
    echo $error_message;
    
    header("refresh:0;url=viewSurveyList.php");
    
}


//Obtain survey question details
    $query = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID ORDER BY questionNumber ASC");
    $query->bindParam(":surveyID", $surveyID);
    $query->execute();
    $result = $query->fetchAll(); 

function getAnswersList($questionID){
    $db_handle = new DBController();
    //Obtain question dropdown/checkbox/radio answers
    $query = $db_handle->getConn()->prepare("SELECT * FROM survey_questionanswer WHERE questionID = :questionID ORDER BY answerTitle");
    $query->bindParam(":questionID", $questionID);
    $query->execute();
    $answersresult = $query->fetchAll();

    return $answersresult;
}

/* Processing to insert user's survey answers into database */
$errorPresence = false;
$errorMessage = "";
$resultMsg = "";



if(!isset($_SESSION["normaluser_login"])) {
    $errorPresence = true;
    $errorMessage = "Please log in to attempt the survey. Thank you.";
    header("refresh:2;url=login.php");
}else{
    $username = $_SESSION["normaluser_login"];
    try{
        
        //Validate if user has participated before
        $selectQuery = $db_handle->getConn()->prepare("SELECT userID FROM survey_response WHERE surveyID = :surveyID  AND userID = :userID");
        $selectQuery->bindParam(":surveyID", $surveyID);
        $selectQuery->bindParam(":userID", $_SESSION["normaluser_login"]);
        $selectQuery->execute();
        $selectResult = $selectQuery->fetchAll();
        $total = count($selectResult);
        
        if ($total > 0) {
            $errorPresence = true;
            $errorMessage = "You have already participated before. Thank you for your participation!";
            header("refresh:2;url=index.php");
        }else{
            
            if(isset($_POST["formsubmitted"])){
                
                //Generate unique ID
                $digits = 7;
                $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                $isIDUnique = false;

                //Check for survey_response ID uniqueness  
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
                
                //Insert survey_response id into database
                $insertResponseQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response (responseID, userID, surveyID, dateSubmitted) VALUES
                (:responseID, :userID, :surveyID, NOW())");

                $insertResponseQuery->bindParam(":responseID", $responseID);
                $insertResponseQuery->bindParam(":userID", $username);
                $insertResponseQuery->bindParam(":surveyID", $surveyID);
                $insertResult = $insertResponseQuery->execute();
                
                if($insertResult){
                    $currentquestionindex = 0;
                    //Insert survey response into database
                    foreach($result as $key => $question){
                        $currentquestionID = $question["questionID"];
                        //Keep track of question number in order to access POST[inputname] from the form
                        $questionname = "question" . $currentquestionindex . "answer";
                        
                        //Generate unique ID for answer
                        $digits = 7;
                        $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                        $isIDUnique = false;
                        
                        //Check for survey_response_answer ID uniqueness  
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
                        
                        if(isset($_POST[$questionname])){
                            //if post name is an array, then its checkbox
                            if(is_array($_POST[$questionname]) == true){
                                $answers = $_POST[$questionname];

                                foreach($answers as $answerID){
                                    $inputanswerID = sanitizeInput($answerID);
                                    $checkboxsononeedtextinput = "";

                                    $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                                    $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                                    $insertQuery->bindParam(":responseID", $responseID);
                                    $insertQuery->bindParam(":questionID", $currentquestionID);
                                    $insertQuery->bindParam(":answerID", $inputanswerID);
                                    $insertQuery->bindParam(":text_answer", $checkboxsononeedtextinput);

                                    $insertResult = $insertQuery->execute();

                                    if (!$insertResult) {
                                        $errorPresence = true;
                                        $errorMessage = "Failed to submit checkbox selections.";
                                    }

                                    //Check for survey_response_answer ID uniqueness  
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
                            }else if($question["questionType"] == "shortsentence" || $question["questionType"] == "longsentence"){
                                $answerID = "";
                                $text_answer = sanitizeInput($_POST[$questionname]);

                                $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                                $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                                $insertQuery->bindParam(":responseID", $responseID);
                                $insertQuery->bindParam(":questionID", $currentquestionID);
                                $insertQuery->bindParam(":answerID", $answerID);
                                $insertQuery->bindParam(":text_answer", $text_answer);

                                $insertResult = $insertQuery->execute();

                                if (!$insertResult) {
                                    $errorPresence = true;
                                    $errorMessage = "Failed to submit text answer.";
                                }
                            }else{
                                $inputanswerID = sanitizeInput($_POST[$questionname]);
                                $radiodropdownsononeedtextinput = "";
                                $insertQuery = $db_handle->getConn()->prepare("INSERT INTO survey_response_answer (resp_answerID, responseID, questionID, answerID, text_answer) VALUES (:resp_answerID, :responseID, :questionID, :answerID, :text_answer)");
                                $insertQuery->bindParam(":resp_answerID", $resp_answerID);
                                $insertQuery->bindParam(":responseID", $responseID);
                                $insertQuery->bindParam(":questionID", $currentquestionID);
                                $insertQuery->bindParam(":answerID", $inputanswerID);
                                $insertQuery->bindParam(":text_answer", $radiodropdownsononeedtextinput);

                                $insertResult = $insertQuery->execute();

                                if (!$insertResult) {
                                    $errorPresence = true;
                                    $errorMessage = "Failed to submit radiobutton or dropdown selection.";
                                }
                            }
                        }
                        $currentquestionindex++;
                    }
                }
                if (!$errorPresence) {
                    $resultMsg = "Successfully submitted survey. Thank you for participating!";
                    header("refresh:2;url=index.php");
                } else {
                    $resultMsg = "Error occured while submitting survey. Please try again!";
                }
            }
        }
    }catch(PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
}






?>
