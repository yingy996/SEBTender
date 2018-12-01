<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

//Check which information to return
//if(isset($_POST["infoToObtain"])) {
    //if($_POST["infoToObtain"] == "surveys"){
        //Obtain survey question details
        $query = $db_handle->getConn()->prepare("SELECT * FROM survey WHERE isEnded = 0 AND isDeleted = 0");
        $query->execute();
        $result = $query->fetchAll();
        
        //If there are survey(s) present, obtain the survey details, else, display error message
        if($result[0][0] = "") {
            $error_message = "There is no surveys for now.";
        }
    /*} else if($_POST["infoToObtain"] == "questions"){
        if(isset($_POST["surveyID"])) {
            $surveyID = sanitizeInput($_POST["surveyID"]);
            //Obtain survey question details
            $query = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID ORDER BY questionNumber ASC");
            $query->bindParam(":surveyID", $surveyID);
            $query->execute();
            $result = $query->fetchAll();
            
            if($result[0][0] != ""){
                $resultString = json_encode($result);
                echo $resultString;
            }            
        } else {
            $error_message = "No survey ID provided";
            echo $error_message;
        }
    }else if($_POST["infoToObtain"] == "answers"){
        if(isset($_POST["questionID"])){
            $questionID = sanitizeInput($_POST["questionID"]);
            //Obtain question dropdown/checkbox/radio answers
            $query = $db_handle->getConn()->prepare("SELECT * FROM survey_questionanswer WHERE questionID = :questionID ORDER BY answerTitle");
            $query->bindParam(":questionID", $questionID);
            $query->execute();
            $result = $query->fetchAll();
            
            if($result[0][0] != ""){
                $resultString = json_encode($result);
                echo $resultString;
            }
            
            
        }else{
            $error_message = "No question ID provided";
            echo $error_message;
        }
    }*/
//}


?>