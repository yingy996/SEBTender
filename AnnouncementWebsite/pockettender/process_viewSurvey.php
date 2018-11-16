<?php 
	require_once("../dbcontroller.php");
	$db_handle = new DBController();

    if(isset($_POST["username"]) && isset($_POST["password"])) {
        //if received request from application, check user login status and return result
        try {
            $adminUsername = $_POST["username"];
            $adminPassword = $_POST["password"];
            
            $db_handle = new DBController();
            /* Get number of rows where user input username matches with those in database */
            $query = $db_handle->getConn()->prepare("SELECT username, password FROM administrator WHERE username = :username");

            $query->bindParam(":username", $adminUsername);
            $query->execute();
            $result = $query->fetchAll();
            $total = count($result);
            
            if ($total > 0) {
                //Validate user password
                if(password_verify($adminPassword, $result[0]["password"])){
                    //if user authentication succeeded, return the result
                    $selectQuery = $db_handle->getConn()->prepare("SELECT * FROM survey ORDER BY startDate ASC");
                    $selectQuery->execute();
                    $results = $selectQuery->fetchAll();
                    
                    if(!empty($results)) {
                        $returnResult = json_encode($results);
                        echo $returnResult;
                    } else {
                        echo "No survey found";
                    }
                } else {
                    echo "Invalid password"; //login failed
                }
            } else {
                echo "User not found!";
            }
        } catch (PDOException $e) {
            echo "Error: " . $e->getMessage();
        }
    } else {
        if(isset($_SESSION["user_login"])){
            $query = $db_handle->getConn()->prepare("SELECT * FROM survey ORDER BY startDate DESC");
            $query->execute();
            $results = $query->fetchAll();
            if (isset($_POST["deleteSurveyButton"])) {
                    $deleteSurveyID = $_POST["surveyID"];

                    $query = $db_handle->getConn()->prepare("SELECT * FROM survey WHERE surveyID = :deleteSurveyID AND isDeleted = 0");
                    $query->bindParam(":deleteSurveyID", $deleteSurveyID);
                    $query->execute();
                    $selectResult = $query->fetchAll();

                    //If survey exists
                    if($selectResult[0][0] != "")
                    {
                        $query = $db_handle->getConn()->prepare("UPDATE survey SET isDeleted = 1, isEnded = 1 WHERE surveyID = :deleteSurveyID");
                        $query->bindParam(":deleteSurveyID", $deleteSurveyID);
                        $result = $query->execute();

                        if($result == true) {
                            echo "<script type='text/javascript'>alert('Survey has been successfully deleted.');</script>";
                            header("refresh:0, viewSurvey.php");
                        } else {
                            echo "<script type='text/javascript'>alert('Error occured while trying to delete survey. Please try again.');</script>";
                            header("refresh:0, viewSurvey.php");
                        }

                    } else {
                        echo "CANNOT FIND ". $deleteSurveyID . "It might have already been deleted.";
                        header("refresh:2;url=viewSurvey.php");
                    }
            }
        }else{
            header("Location: index.php");
        }
    }    
?>
