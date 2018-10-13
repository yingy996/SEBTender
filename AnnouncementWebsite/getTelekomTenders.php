<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>Telekom tender</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg"> <!--full page background img -->
        <?php 
        include("header.php");
        
        ?>

        <div class="container container-fluid">
            <label>
<?php

require("simple_html_dom.php");
require("tender_object.php");
findMyProcurementTenders();
        
//Get all  tenders from myProcurement
function findMyProcurementTenders(){
    $currenthtmlDoc = file_get_html("https://www.tm.com.my/DoingBusinessWithTM/pages/notices.aspx?Year=2018");
    $htmlNodes = $currenthtmlDoc->find("//table");
    $telekomTenders = array();

    $count = 0;
    $currentMonth = date("F");
    $lastMonth = date("F", strtotime("-1 month"));
    //echo "Last month: " . strtolower($lastMonth);
    //echo "Current month: " . strtolower($currentMonth);
    foreach($htmlNodes as $tableNode){
        if ($count != 0) {
            $nodeCount = 0;
            $tenderMonth = $tableNode->parent()->prev_sibling()->innertext;

            //Only display the tenders published in current month
            if (strtolower($currentMonth) == strtolower($tenderMonth) || strtolower($lastMonth) == strtolower($tenderMonth)) {
                $trNodes = $tableNode->find("tr");
                foreach ($trNodes as $trNode) {
                    if (trim($trNode->outertext) != ""){
                        //Skip 0 as the first tr node is the title of the list
                        if ($nodeCount != 0) {
                            $tdCount = 0;
                            //Instantiate a tender object 
                            $tenderObject = new scrapped_tender();
                            
                            $tdNodes = $trNode->find("td");
                            foreach($tdNodes as $tdNode) {
                                if (trim($tdNode->innertext) != "") {
                                    switch ($tdCount) {
                                        case 0:
                                            //Published date
                                            //echo "Published date: " . $tdNode->innertext;
                                            $tenderObject->startDate = $tdNode->innertext;
                                            break;
                                            
                                        case 1:
                                            //echo "Title: " . $tdNode->innertext;
                                            $tenderObject->title = $tdNode->innertext;
                                            break;
                                            
                                        case 2:
                                            foreach ($tdNode->children as $childNode){
                                                if (trim($childNode) != "") {
                                                    if (isset($childNode->href)) {
                                                        $fileName = trim($childNode->innertext);
                                                        $link = $childNode->href;
                                                        //echo "Link: " . $tdNode->innertext; 
                                                        //echo "Link: " . $link; 
                                                        $tenderObject->fileLink = $tdNode->innertext;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    $tdCount++;
                                }
                            }
                            //Set tender source
                            $tenderObject->originatingSource = "Telekom";
                            $tenderObject->tenderSource = 2;
                            //Add the tender object to array
                            $telekomTenders[] = $tenderObject;
                        }
                        $nodeCount++;
                    }
                }
            }
        }
        $count++;
    }
    if (count($telekomTenders) > 0) {
        insertIntoDatabase($telekomTenders);
    }
}

//function to loop through list of tender items in $scrapped_tenders and insert into database
function insertIntoDatabase($scrapped_tenders){
    require_once("dbcontroller.php");
    $db_handle = new DBController();
    $result = "";
    $count = 0;
    while(array_key_exists($count, $scrapped_tenders)){
        $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (title, originatingSource, tenderSource, startDate, fileLinks) VALUES
        (:title, :originatingSource, 2, :startDate, :fileLinks)");
        $query->bindParam(":title", $scrapped_tenders[$count]->title);
        $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
        $query->bindParam(":startDate", $scrapped_tenders[$count]->startDate);
        $query->bindParam(":fileLinks", $scrapped_tenders[$count]->fileLink);

        $result = $query->execute();
        if($result == true){
            $count++;
        }
    }
    
    if ($result) {
        echo "Tenders have been successfully stored!";
    }
}                

// Function for basic field validation (present and neither empty nor only white space
function IsNullOrEmptyString($str){
    return (!isset($str) || trim($str) === '');
}

?>
</label>
        </div>

        <!-- Footer -->
        <?php 
        include("footer.php");
        ?>

        <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
        <script src="js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="js/bootstrap.min.js"></script>
        <!--Basic AngularJS-->

    </body>
</html>