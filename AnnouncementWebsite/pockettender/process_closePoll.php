<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

if (!isset($_POST["close_pollID"])) {
    header("location: index.php");
}

if (!isset($_POST["login_user"])) {
    header("location: login.php");
} else {
    $login_user = sanitizeInput($_POST["login_user"]);
}

if (!empty($_POST["close_pollID"])) {
    $pollID = sanitizeInput($_POST["close_pollID"]);
    
    $query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE pollID = :pollID");
    $query->bindParam(":pollID", $pollID);
    $query->execute();
    $pollResult = $query->fetchAll();
    
    if ($pollResult != null) {
        //Close the selected poll
        $updateQuery = $db_handle->getConn()->prepare("UPDATE poll_question SET isEnded = 1, editedDate = NOW(), editedBy = :login_user, endDate = NOW() WHERE pollID = :pollID");
        $updateQuery->bindParam(":pollID", $pollID);
        $updateQuery->bindParam(":login_user", $login_user);

        $result = $updateQuery->execute();
        
        if ($result) {
            echo "<script type='text/javascript'>alert('Poll has been successfully closed.');</script>";
            header("refresh:0, index.php");
        } else {
            echo "<script type='text/javascript'>alert('Error occured while trying to close the poll. Please try again.');</script>";
            header("refresh:0, index.php");
            //$error_message = "Problem in editing announcement. Try Again!";	
        }
    } else {
        echo "<script type='text/javascript'>alert('Poll not found! Please try again.');</script>";
    }
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>