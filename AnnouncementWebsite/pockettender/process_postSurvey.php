<?php
if (isset($_SESSION["user_login"])) {
    $survey_title = "";
    $survey_titleerr = "";
    $survey_description = "";
    $survey_descriptionerr = "";
    
    $question_title = array();
    $question_titleerr = "";
    $answer_type = array();
    $answer_typeerr = "";
    $answer_title = array();
    $answer_titleerr = "";
    
    $end_date = "";
    $end_dateerr = "";

    
    $errorpresence = false;
    $error_message = "";
    
    if(!empty($_POST["submitsurvey"])) {
    
    
        /* Survey title Validation */
        if(empty($_POST["title"])){
            $survey_titleerr = "Please enter a survey title";
            $errorpresence = true;
        }else{
            $survey_title = sanitizeInput($_POST["title"]);
        }
		
        /* Survey description Validation */
        if(empty($_POST["content"])){
            $survey_descriptionerr = "Please enter a survey description";
            $errorpresence = true;
        }else{
            $survey_description = sanitizeInput($_POST["content"]);
        }
        
        /* Get all user inputs and prepare into variable for later insertion into database */
        $questioncount = 0;
        $nextquestionavailable = true;
        while($nextquestionavailable == true){
            $questiontitleName = "question_title0" . $questioncount;
            $answertypeName = "answertype0" . $questioncount;
            if(isset($_POST[$questiontitleName]) && !empty($_POST[$questiontitleName])){
                $question_title[$questiontitleName] = sanitizeInput($_POST[$questiontitleName]);
                $answer_type[$answertypeName] = sanitizeInput($_POST[$answertypeName]);
                
                $answercount = 0;
                $nextansweravailable = true;
                while($nextansweravailable == true){
                    $answertitleName = "answer_title0" . $questioncount . "0" . $answercount;
                    if(isset($_POST[$answertitleName]) && !empty($_POST[$answertitleName])){
                        $answer_title[$answertitleName] = sanitizeInput($_POST[$answertitleName]);
                        
                        $answercount++;
                    }elseif(isset($_POST[$answertitleName]) && empty($_POST[$answertitleName])){
                        $answer_titleerr = "Answer title cannot be blank";
                        $errorpresence = true;
                        $nextansweravailable = false;
                    }else{
                        $nextansweravailable = false;
                    }
                }
                $questioncount++;
            }elseif(isset($_POST[$questiontitleName]) && empty($_POST[$questiontitleName])){
                $question_titleerr = "Question title cannot be blank";
                $errorpresence = true;
                $nextquestionavailable = false;
            }else{
                $nextquestionavailable = false;
            }
        }
        
        /* Date ended Validation */
        if(empty($_POST["enddate"])){
            $end_dateerr = "Please enter a end date for this survey";
            $errorpresence = true;
        }else{
            $end_date = sanitizeInput($_POST["enddate"]);
        }
        
        if($error_message == "" && $errorpresence == false){
            require_once("../dbcontroller.php");
            $db_handle = new DBController();
            
            $digits = 7;
            $randomsurveyID = rand(pow(10, $digits-1), pow(10, $digits)-1);
            
            $isSurveyIDUnique = false;
            //Check uniqueness of survey ID, if unique then proceed if not change
            while($isSurveyIDUnique == false){
                $selectsurveyquery = $db_handle->getConn()->prepare("SELECT surveyID FROM survey WHERE surveyID = :randomsurveyID");
                $selectsurveyquery->bindParam(":randomsurveyID", $randomsurveyID);
                $selectsurveyquery->execute();
                $selectsurveyResult = $selectsurveyquery->fetchAll();
                $total = count($selectsurveyResult);
                if($total == 0){
                    $isSurveyIDUnique = true;
                }else{
                    $randomsurveyID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                }
            }
            
            //Insert surveys into database
            $surveyquery = $db_handle->getConn()->prepare("INSERT INTO survey (surveyID, surveyTitle, description, publishedBy, startDate, endDate, noOfResponse, isEnded) VALUES (:randomsurveyID, :survey_title, :survey_description, :login_user, NOW(), :end_date, :numberofresponse, :isended)");
                
            $surveyquery->bindParam(":randomsurveyID", $randomsurveyID);
            $surveyquery->bindParam(":survey_title", $survey_title);
            $surveyquery->bindParam(":survey_description", $survey_description);
            $surveyquery->bindParam(":login_user", $login_user);
            $surveyquery->bindParam(":end_date", $end_date);
            $numberofresponse = 0;
            $surveyquery->bindParam(":numberofresponse", $numberofresponse);
            $isended = 0;
            $surveyquery->bindParam(":isended", $isended);
            
            $surveyresult = $surveyquery->execute();
            if($surveyresult == true){
                for($i=0; $i<count($question_title); $i++){
                    $questiontitleName = "question_title0" . $i;
                    $currentquestionTitle = $question_title[$questiontitleName];
                    $answertypeName = "answertype0" . $i;
                    $currentanswerType = $answer_type[$answertypeName]; 
                    $randomquestionID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                    
                    /* Check uniquness of question id, if unique then proceed*/
                    $isQuestionIDUnique = false;
                    /* Check uniquness of question id, if unique then proceed*/
                    
                    while($isQuestionIDUnique == false){
                        $selectquestionquery = $db_handle->getConn()->prepare("SELECT questionID FROM survey_question WHERE questionID = :randomquestionID");
                        $selectquestionquery->bindParam(":randomquestionID", $randomquestionID);
                        $selectquestionquery->execute();
                        $selectquestionResult = $selectquestionquery->fetchAll();
                        $total = count($selectquestionResult);
                        if($total == 0){
                            $isQuestionIDUnique = true;
                        }else{
                            $randomquestionID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                        }
                    }
                    //Insert questions into database
                     $questionquery = $db_handle->getConn()->prepare("INSERT INTO survey_question (questionID, questionTitle, surveyID, questionType, questionNumber) VALUES (:randomquestionID, :currentquestionTitle, :randomsurveyID, :currentanswerType, :questionNumber)");
                    
                    $questionquery->bindParam(":randomquestionID", $randomquestionID);
                    $questionquery->bindParam(":currentquestionTitle", $currentquestionTitle);
                    $questionquery->bindParam(":randomsurveyID", $randomsurveyID);
                    $questionquery->bindParam(":currentanswerType", $currentanswerType);
                    $questionquery->bindParam(":questionNumber", $i);
                    
                    $questionresult = $questionquery->execute();
                    
                    
                    if($questionresult == true){
                        $answercount = 0;
                        $nextansweravailable = true;
                        
                        while($nextansweravailable == true){
                            $answertitleName = "answer_title0" . $i . "0" . $answercount;
                            if(array_key_exists($answertitleName, $answer_title)){
                                $currentanswerTitle = $answer_title[$answertitleName];
                                
                                $randomanswerID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                                
                                $isAnswerIDUnique = false;
                                //Check uniquness of answer ID
                                    while($isAnswerIDUnique == false){
                                        $selectanswerquery = $db_handle->getConn()->prepare("SELECT answerID FROM survey_questionanswer WHERE answerID = :randomanswerID");
                                        $selectanswerquery->bindParam(":randomanswerID", $randomanswerID);
                                        $selectanswerquery->execute();
                                        $selectanswerResult = $selectanswerquery->fetchAll();
                                        $total = count($selectanswerResult);
                                        if($total == 0){
                                            $isAnswerIDUnique = true;
                                        }else{
                                            $randomanswerID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                                        }
                                    }
                                
                                //Insert answer options into database
                                $answerquery = $db_handle->getConn()->prepare("INSERT INTO survey_questionanswer (answerID, answerTitle, questionID, surveyID) VALUES (:randomanswerID, :currentanswerTitle, :randomquestionID, :randomsurveyID)");
                                
                                $answerquery->bindParam(":randomanswerID", $randomanswerID);
                                $answerquery->bindParam(":currentanswerTitle", $currentanswerTitle);
                                $answerquery->bindParam(":randomquestionID", $randomquestionID);
                                $answerquery->bindParam(":randomsurveyID", $randomsurveyID);

                                $answerresult = $answerquery->execute();
                                if($answerresult == false){
                                    $error_message = "Failed to submit";
                                }
                                $answercount++;
                            }else{
                                $nextansweravailable = false;
                            }
                        }
                        $error_message = "";
                        $success_message = "You have succesfully published the survey!";
                        unset($_POST);
                        //header("refresh:2; url=index.php");
                    }else{
                        $error_message = "Failed to submit";
                    }
                }
            }
            
        }
        
    }
} else {
    header("location: login.php");
    exit();
}
?>