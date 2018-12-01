<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if(isset($_POST["dataToRetrieve"])) {
    $dataToRetrieve = sanitizeInput($_POST["dataToRetrieve"]);
    
    if ($dataToRetrieve == "originating source") {
        //Retrieve originating sources from database
        $query = $db_handle->getConn()->prepare("SELECT originatingSource FROM scrapped_tender GROUP BY originatingSource");
        $query->execute();
        $result = $query->fetchAll();
        
        if ($result != null) {
            $sourceArray = array();
            foreach ($result as $source) {
                $sourceArray[] = $source["originatingSource"];
            }
            $resultString = json_encode($sourceArray);
            echo $resultString;
        } else {
            echo "Originating sources not found";
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