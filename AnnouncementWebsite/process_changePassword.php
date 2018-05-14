<?php
$oldPasswordError = $newPasswordError = $confPasswordError = "";
$errorpresence = false;
$newPassword = "";
$username = $login_user;
$oldPassword = $newPassword = "";

if(isset($_POST["updateBtn"])){
    //Validate inputs
    if(empty($_POST["oldPassword"])){
        $oldPasswordError = "Old Password must not be empty";
        $errorpresence = true;
    } else {
        $oldPassword = $_POST["oldPassword"];
    }
    
    if(empty($_POST["newPassword"])){
        $newPasswordError = "New Password must not be empty";
        $errorpresence = true;
    }

    if($_POST["newPassword"] != $_POST["confPassword"]){ 
        $confPasswordError = "Confirm Password does not match"; 
        $errorpresence = true;
    } else {
        $newPassword = password_hash(sanitizeInput($_POST["newPassword"]), PASSWORD_DEFAULT);
    }

    if(empty($_POST["confPassword"])){
        $confPasswordError = "Confirm Password must not be empty";
        $errorpresence = true;
    }
    
    //Validate if old password is correct
    $db_handle = new DBController();
    $query = $db_handle->getConn()->prepare("SELECT password FROM administrator WHERE username = :username");
    $query->bindParam(":username", $username);

    $query->execute();
    $result = $query->fetchColumn();
    
    
    if(!password_verify($oldPassword, $result)){
        $oldPasswordError = "Old password is incorrect";
        $errorpresence = true;
    }
    
    if (!$errorpresence) {
        //if no error found, update the password
        $updateQuery = $db_handle->getConn()->prepare("UPDATE administrator SET password = :password WHERE username = :username");
        $updateQuery->bindParam(":username", $username);
        $updateQuery->bindParam(":password", $newPassword);
        $updateResult = $updateQuery->execute();
        
        if ($updateResult) {
            $success_message = "Successfully changed password!";
            header("refresh:2;url=manage_profile.php");
        }
    }
}

?>