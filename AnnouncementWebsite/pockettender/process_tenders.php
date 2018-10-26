<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

$query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender");
$query->execute();
$results = $query->fetchAll();

?>