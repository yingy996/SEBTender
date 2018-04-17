<?php
class DBController {
	private $host = "localhost";
	private $user = "root";
	private $password = "";
	private $database = "sarawaktenderapplication_db";
	private $conn;
	
	function __construct() {
		$this->conn = $this->connectDB();
	}
	
	function connectDB() {
        
        try{
            $opt = array(PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION);
            $conn = new PDO("mysql:host=localhost;dbname=sarawaktenderapplication_db;charset=utf8mb4", "root", "", $opt);
            error_reporting(E_ALL);
            ini_set("display_errors",1);

            #$conn = mysqli_connect($this->host,$this->user,$this->password,$this->database);
            return $conn;
        }catch(PDOException $e){
            echo $e->getMessage();
        }
	}
	
    function getConn() {
        return $this->conn;
    }
    
	function runQuery($query){
        $result = $this->conn->query($query);
        while($row=$result->fetch(PDO::FETCH_ASSOC)){
            $resultset[]=$row;
        }
        if(!empty($resultset))
            return $resultset;
        
        
        
		#$result = mysqli_query($this->conn,$query);
        #while($row=mysqli_fetch_assoc($result)) {
		#	$resultset[] = $row;
		#}		
		#if(!empty($resultset))
		#	return $resultset;
	}
	
	function numRows($query) {
        #$rowcount = $this->conn->exec($query)->fetchColumn(); exec returns number of rows for insert, update, delete. query function returns pdostatement object which can be manipulated by other pdo functions such as fetchcolumn
        $rowcount = $this->conn->query($query)->fetchColumn();
       
        
		#$result  = mysqli_query($this->conn,$query);
		#$rowcount = mysqli_num_rows($result);
		return $rowcount;	
	}
	
	function updateQuery($query) {
        $result = $this->conn->exec($query);
        return $result;
        
        
        
		#$result = mysqli_query($this->conn,$query);
		#if (!$result) {
		#	die('Invalid query: ' . mysql_error());
		#} else {
		#	return $result;
		#}
	}
	
	function insertQuery($query) {
        $result = $this->conn->exec($query);
        return $result;
        
        
        
        
		#$result = mysqli_query($this->conn,$query);
		#if (!$result) {
		#	die('Invalid query: ' . mysql_error());
		#} else {
		#	return $result;
		#}
	}
	
	function deleteQuery($query) {
        $result = $this->conn->exec($query);
        return $result;
        
        
        
        
		#$result = mysqli_query($this->conn,$query);
		#if (!$result) {
		#	die('Invalid query: ' . mysql_error());
		#} else {
		#	return $result;
		#}
	}
}
?>