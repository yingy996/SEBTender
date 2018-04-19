<?php 
	//include_once("dbcontroller.php");
    @ob_start();
	require_once("dbcontroller.php");
    if (session_status() == PHP_SESSION_NONE) {
        @session_start();
    }

    if(isset($_SESSION["user_login"])) {
        $user_check = $_SESSION["user_login"];
		
		$db_handle = new DBController();
		//check if username exits
		$query = $db_handle->getConn()->prepare("SELECT username FROM administrator WHERE username = :username");
        $query->bindParam(":username", $user_check);
		
		$user_check = sanitizeInput($user_check);
		
		$query->execute();
		$result = $query->fetchAll();
		$login_user = $result[0]["username"];
    } 
    
    /*if(!isset($_SESSION["user_login"])) {
        if (basename($_SERVER["PHP_SELF"]) == "addproduct.php" || basename($_SERVER["PHP_SELF"]) == "viewuseraccount.php" || basename($_SERVER["PHP_SELF"]) == "edituseraccount.php" || basename($_SERVER["PHP_SELF"]) == "productdetails.php") {
			header("location: login.php");
        	exit;
		}
		
    }*/

	function sanitizeInput($data) {
        $data = trim($data);
        $data = stripslashes($data);
        $data = htmlspecialchars($data);
        
        return $data;
    }

?>