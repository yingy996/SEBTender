<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

//Check which information to return
if(isset($_POST["infoToObtain"])) {
    if($_POST["infoToObtain"] == "question"){
        //Obtain poll question details
        $query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE isEnded = 0");
        $query->execute();
        $result = $query->fetchAll();
        
        //If there are poll present, obtain the poll details, else, display error message
        if($result != null) {
            $resultString = json_encode($result);
            echo $resultString;
        } else {
            $error_message = "There is no poll for now.";
            if (isset($_SESSION["user_login"])) {
                $error_message += "Create a new poll?";
            }
            echo $error_message;
        }
    } else {
        if(isset($_POST["pollID"])) {
            $pollID = sanitizeInput($_POST["pollID"]);
            //Obtain poll options details
            $query = $db_handle->getConn()->prepare("SELECT * FROM poll_option WHERE pollID = :pollID");
            $query->bindParam(":pollID", $pollID);
            $query->execute();
            $result = $query->fetchAll();
            
            if($result[0][0] != ""){
                $resultString = json_encode($result);
                echo $resultString;
            }            
        } else {
            $error_message = "No poll ID provided";
            echo $error_message;
        }
    }
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>