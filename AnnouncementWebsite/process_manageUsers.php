<?php 
	require_once("dbcontroller.php");
	$db_handle = new DBController();

    $query = $db_handle->getConn()->prepare("SELECT username, administratorName, administratorEmail, role FROM administrator WHERE isDeleted = false ORDER BY administratorName ASC");
    $query->execute();
    $results = $query->fetchAll();
?>
