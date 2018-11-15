<?php 
	//include_once("dbcontroller.php");
    @ob_start();
	require_once("../dbcontroller.php");
    if (session_status() == PHP_SESSION_NONE) {
        @session_start();
    }
    
    if(isset($_SESSION["normaluser_login"])) {
        $user_check = $_SESSION["normaluser_login"];
		
		$db_handle = new DBController();
		//check if username exits
		$query = $db_handle->getConn()->prepare("SELECT username FROM user WHERE username = :username");
        $query->bindParam(":username", $user_check);
		
		$user_check = sanitizeInput($user_check);
		
		$query->execute();
		$result = $query->fetchAll();
		$login_user = $result[0]["username"];
        
    }else if(isset($_SESSION["user_login"])){
        //administrator log in
        $user_check = $_SESSION["user_login"];
		
		$db_handle = new DBController();
		//check if username exits
		$query = $db_handle->getConn()->prepare("SELECT username, role FROM administrator WHERE username = :username");
        $query->bindParam(":username", $user_check);
		
		$user_check = sanitizeInput($user_check);
		
		$query->execute();
		$result = $query->fetchAll();
		$login_user = $result[0]["username"];
        $login_userRole = $result[0]["role"];
    }
?>