<?php
require("survey_object.php");
require_once("dbcontroller.php");
$db_handle = new DBController();

//Retrieve survey results from database
if (isset($_GET["surveyID"]) || isset($surveyID)) {
    //Retrieve survey information of selected survey from database
    //Obtain survey question details
    $surveyID = sanitizeInput($_GET["surveyID"]);

    $query = $db_handle->getConn()->prepare("SELECT * FROM survey_response WHERE surveyID = :surveyID");
    $query->bindParam(":surveyID", $surveyID);
    
    $query->execute();
    $results = $query->fetchAll();
    
    if (count($results) > 0) { 
        //For each of the response, retrieve the response answers
        $count = 0;
        $file = fopen("survey". $surveyID .".csv","w");
        
        foreach ($results as $response) {
            //Create a survey_response object
            $surveyResponseObj = new survey_response();
            
            $surveyResponseObj->id = $count;
            $surveyResponseObj->responseID = $response["responseID"];
            $surveyResponseObj->userID = $response["userID"];
            $surveyResponseObj->dateSubmitted = $response["dateSubmitted"];

            $responseQuery = $db_handle->getConn()->prepare("SELECT * FROM survey_response_answer WHERE responseID = :responseID ORDER BY questionID");
            $responseQuery->bindParam(":responseID", $response["responseID"]);

            $responseQuery->execute();
            $responseAnswers = $responseQuery->fetchAll();

            if (count($responseAnswers) > 0) {
                foreach ($responseAnswers as $answer) { 
                    $questionID = $answer["questionID"];
                    if ($answer["answerID"] == 0) {
                        //Save text answer
                        $questionID = "question_" . $questionID;
                        $surveyResponseObj->$questionID = $answer["text_answer"];
                    } else {
                        //Get answer from survey_questionanswer table
                        $answerID = $answer["answerID"];
                        $answerQuery = $db_handle->getConn()->prepare("SELECT * FROM survey_questionanswer WHERE answerID = :answerID AND questionID = :questionID");
                        $answerQuery->bindParam(":answerID", $answerID);
                        $answerQuery->bindParam(":questionID", $questionID);

                        $answerQuery->execute();
                        $answerResults = $answerQuery->fetchAll();
                        
                        if (count($answerResults) > 0) {
                            $questionID = "question_" . $questionID;
                            $surveyResponseObj->$questionID = $answerResults[0]["answerTitle"];
                        }
                    }
                }
            }            
    
            $jsonResponse = json_encode($surveyResponseObj);
            echo $jsonResponse;
            echo "<hr/>";
            $assocResponse = json_decode($jsonResponse, true);
            if ($count == 0) {
                $firstLineKeys = array_keys($assocResponse);
                fputcsv($file, $firstLineKeys);
            }
            fputcsv($file, $assocResponse);
            
            $count++;
        }
        fclose($file);
        //Generate excel file for mapping survey question ID to the question
        //Retrieve survey question information
        $questionQuery = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID");
        $questionQuery->bindParam(":surveyID", $surveyID);

        $questionQuery->execute();
        $questionResult = $questionQuery->fetchAll();
        
        if (count($questionResult) > 0) {
            $file = fopen("surveyQuestions_". $surveyID .".csv","w");
            $question = array();
            $question[] = "id_,_title"; 
            foreach ($questionResult as $questionInfo) {
                $question[] = $questionInfo["questionID"] . "_,_" . $questionInfo["questionTitle"];                    
            }
            
            foreach ($question as $line) {
                fputcsv($file,explode("_,_",$line));
            }
            fclose($file);
        }
        
        
        //echo json_encode($responsesArr);
        //Convert results into json object
        //Convert json object to associative array

        //Convert from array to CSV file
        
        /*$firstLineKeys = false;
        foreach($assocResponse as $line) {
            if (empty($firstLineKeys)) {
                $firstLineKeys = array_keys($line);
                fputcsv($file, $firstLineKeys);
                $firstLineKeys = array_flip($firstLineKeys);
            }
            fputcsv($file, array_merge($firstLineKeys, $line));
            //fputcsv($file, explode(",", $line));
        } */       
    } else {
        echo "No result";
    }
} else {
    echo "Missing parameter for survey ID";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>