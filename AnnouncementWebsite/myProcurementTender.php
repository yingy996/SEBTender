
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
            <label><?php

require("simple_html_dom.php");
require("tender_object.php");             
findMyProcurementTenders();
 

//Get all  tenders from myProcurement
function findMyProcurementTenders(){
    //list of scrapped_tender object
    
    $scrapped_tenders = array();  
    
    $currenthtmlDoc = file_get_html("http://myprocurement.treasury.gov.my/custom/p_iklan_tender.php");
    
    
    $htmlNodes = $currenthtmlDoc->find("//table/table/tr");
    $result = "";
    
/*$tender_number = array();
$tender_title = array();
$tender_reference = array();
$tender_category = array();
$tender_ministry = array();
$tender_originator = array();
$tender_startingdate = array();
$tender_closingdate = array();*/
    
    while($result != "No next page"){
        
        $count = 0;
        foreach($htmlNodes as $trNode){
            //echo $trNode;
            $tdNodes = $trNode->find("td");
            $tdNodeCount = count($tdNodes);
            
            //Required rows starts after 13
            if($count >=14 && $count <=23){
                $currentTdCount = 0;
                $scrappedTender = new scrapped_tender();
                
                
                //for each td nodes in one tr
                foreach($tdNodes as $tdNode){
                    if(!IsNullOrEmptyString($tdNode)){
                        
                        $innertdnode = $tdNode->innertext;
                        
                        switch($currentTdCount){
                            case 0:
                                //if first column contains a link, this means that the row is past the tender rows and there are no more tenders
                                $link = $tdNode->find('a');
                                if($link !=null){
                                    break 3;
                                }
                                //echo "NUMBER: " . $innertdnode . "<br/>";
                                //array_push($tender_number,$innertdnode);
                                //$scrappedTender->reference = $innertdnode;
                                break;

                            case 1:
                                //echo "TITLE: " . $innertdnode . "<br/>";
                                //array_push($tender_title,$innertdnode);
                                $scrappedTender->title = strip_tags($innertdnode);
                                //echo $scrappedTender->title;
                                break;

                            case 2:
                                //echo "Reference: " . $innertdnode . "<br/>";
                                //array_push($tender_reference,$innertdnode);
                                $scrappedTender->reference = strip_tags($innertdnode);
                                break;

                            case 3:
                                //echo "Kategori Perolehan: " . $innertdnode . "<br/>";
                                //array_push($tender_category,$innertdnode);
                                $scrappedTender->category = strip_tags($innertdnode);
                                break;

                            case 4:
                                //echo "Kementerian: " . $innertdnode . "<br/>";
                                //array_push($tender_ministry,$innertdnode);
                                $scrappedTender->originatingSource = strip_tags($innertdnode);
                                break;

                            case 5:
                                //echo "Agency: " . $innertdnode . "<br/>";
                                //array_push($tender_originator,$innertdnode);
                                $scrappedTender->agency = strip_tags($innertdnode);
                                break;

                            case 6:
                                //echo "StartingDate: " . $innertdnode . "<br/>";
                                //array_push($tender_startingdate,$innertdnode);
                                $scrappedTender->startDate = strip_tags($innertdnode);
                                break;

                            case 7:
                                //echo "ClosingDate: " . $innertdnode . "<br/>";
                                //array_push($tender_closingdate,$innertdnode);
                                $scrappedTender->closingDate = strip_tags($innertdnode);
                                break;
                        }
                        $currentTdCount++;
                    }
                }
                $scrapped_tenders[] = $scrappedTender;
                $count++;
            }else{
                $count++;
            }

        }
        
        $result = getNextPageLink($currenthtmlDoc);
        if($result == "No next page"){
            break;
        }
        $currenthtmlDoc = file_get_html("http://myprocurement.treasury.gov.my" . $result);
        $htmlNodes = $currenthtmlDoc->find("//table/table/tr");
        
    }
    
    //call function to insert scrapped tenders into database
    insertIntoDatabase($scrapped_tenders);
}
                
//Check if next page exists
function getNextPageLink($htmlDoc2){
    $nextpagelink = "";
    
    $htmlNodes = $htmlDoc2->find("//table/tr");
    
    $count = 0;
    //foreach trnode
    foreach($htmlNodes as $trNode){
    
    //foreach tdnode 
    $tdNodes = $trNode->find("td");
    $tdNodeCount = count($tdNodes);
    //Required row is at 25
    if($count==25){
        $currentTdCount = 0;
        foreach($tdNodes as $tdNode){
            if(!IsNullOrEmptyString($tdNode)){
                //$innertdnode = $tdNode->innertext;
                switch($currentTdCount){
                    //at td number 3
                    case 3:
                        $linktag = $tdNode->find('a');
                        $nextpagelink = $linktag[0]->href;
                        
                        break;
                }
                
                $currentTdCount++;
            }
        }
        $count++;
    }else{
        $count++;
    }
}
    //if link starts with /cust then returns link, if not then final page is reached
    if(substr($nextpagelink,0,5) === "/cust"){
        
        return $nextpagelink;
        
    }else{
        
        return "No next page";
    }
    
    
}
                
//function to loop through list of tender items in $scrapped_tenders and insert into database
function insertIntoDatabase($scrapped_tenders){
    require_once("dbcontroller.php");
                    $db_handle = new DBController();
                    
                    $count = 0;
                    while(array_key_exists($count,$scrapped_tenders)){
                        $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, title, category, originatingSource, tenderSource, agency, closingDate, startDate) VALUES
                        (:reference, :title, :category, :originatingSource, 0, :agency, STR_TO_DATE(:startDate, '%d/%m/%y'), STR_TO_DATE(:closingDate, '%d/%m/%y'))");
                        
                        $query->bindParam(":reference", $scrapped_tenders[$count]->reference);
                        $query->bindParam(":title", $scrapped_tenders[$count]->title);
                        $query->bindParam(":category", $scrapped_tenders[$count]->category);
                        $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
                        //$query->bindParam(":tenderSource", "0");
                        $query->bindParam(":agency", $scrapped_tenders[$count]->agency);
                        $query->bindParam(":startDate", $scrapped_tenders[$count]->startDate);
                        $query->bindParam(":closingDate", $scrapped_tenders[$count]->closingDate);

                        $result = $query->execute();
                        if($result == true){
                            $count++;
                        }
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