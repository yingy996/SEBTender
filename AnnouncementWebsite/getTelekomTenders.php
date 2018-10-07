<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>myprocurement</title>
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

require('simple_html_dom.php');
        
findMyProcurementTenders();
        
//Get all  tenders from myProcurement
function findMyProcurementTenders(){
    $currenthtmlDoc = file_get_html("https://www.tm.com.my/DoingBusinessWithTM/pages/notices.aspx?Year=2018");
    $htmlNodes = $currenthtmlDoc->find("//table/table/tr");
    $result = "";
    
$tender_number = array();
$tender_title = array();
$tender_reference = array();
$tender_category = array();
$tender_ministry = array();
$tender_originator = array();
$tender_startingdate = array();
$tender_closingdate = array();
    
    while($result != "No next page"){
        $count = 0;

            /*require_once("dbcontroller.php");
                    $db_handle = new DBController();



                    $query = $db_handle->getConn()->prepare("INSERT INTO myprocurementtender (tender_number, tender_title, tender_reference, tender_category, tender_ministry, tender_originator, tender_startingdate, tender_closingdate) VALUES
                    (:randomID, :announcement_title, :announcement_content, NOW(), NULL, NULL, :login_user)");
                    $query->bindParam(":randomID", $randomID);
                    $query->bindParam(":announcement_title", $announcement_title);
                    $query->bindParam(":announcement_content", $announcement_content);
                    $query->bindParam(":login_user", $login_user);

                    $result = $query->execute();*/



        foreach($htmlNodes as $trNode){
            //echo $trNode;
            $tdNodes = $trNode->find("td");
            $tdNodeCount = count($tdNodes);

            //Required rows starts after 13
            if($count >=14 && $count <=23){
                $currentTdCount = 0;
                foreach($tdNodes as $tdNode){
                    if(!IsNullOrEmptyString($tdNode)){
                        $innertdnode = $tdNode->innertext;



                        switch($currentTdCount){
                            case 0:
                                echo "NUMBER: " . $innertdnode . "<br/>";
                                //$tender_number.push($innertdnode);
                                break;

                            case 1:
                                echo "TITLE: " . $innertdnode . "<br/>";
                                //$tender_title.push($innertdnode);
                                break;

                            case 2:
                                echo "Reference: " . $innertdnode . "<br/>";
                                //$tender_reference.push($innertdnode);
                                break;

                            case 3:
                                echo "Kategori Perolehan: " . $innertdnode . "<br/>";
                                //$tender_category.push($innertdnode);
                                break;

                            case 4:
                                echo "Kementerian: " . $innertdnode . "<br/>";
                                //$tender_ministry.push($innertdnode);
                                break;

                            case 5:
                                echo "OriginatingStation: " . $innertdnode . "<br/>";
                                //$tender_originator.push($innertdnode);
                                break;

                            case 6:
                                echo "StartingDate: " . $innertdnode . "<br/>";
                                //$tender_startingdate.push($innertdnode);
                                break;

                            case 7:
                                echo "ClosingDate: " . $innertdnode . "<br/>";
                                //$tender_closingdate.push($innertdnode);
                                break;
                        }
                        echo "<br/>";
                        echo "<br/>";
                        $currentTdCount++;
                    }
                }
                $count++;
            }else{
                $count++;
            }

        }
        
        $result = getNextPageLink($currenthtmlDoc);
        if($result != "No next page"){
            $currenthtmlDoc = file_get_html("http://myprocurement.treasury.gov.my/" . $result);
        }
    }
}
                
//Check if next page exists
function getNextPageLink($htmlDoc2){
    $$nextpagelink = "";
    $htmlNodes = $htmlDoc2->find("//table/tr");
    
    $count = 0;
    foreach($htmlNodes as $trNode){
    
    $tdNodes = $trNode->find("td");
    $tdNodeCount = count($tdNodes);
    //Required row is at 25
    if($count==25){
        $currentTdCount = 0;
        foreach($tdNodes as $tdNode){
            if(!IsNullOrEmptyString($tdNode)){
                //$innertdnode = $tdNode->innertext;
                switch($currentTdCount){

                    case 3:
                        $linktag = $tdNode->find('a');
                        $nextpagelink = $linktag[0]->href;
                        break;
                }
                echo "<br/>";
                echo "<br/>";
                $currentTdCount++;
            }
        }
        $count++;
    }else{
        $count++;
    }
}
    if($finalpagelink == null){
        return "No next page";
    }else{
        return $nextpagelink;
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