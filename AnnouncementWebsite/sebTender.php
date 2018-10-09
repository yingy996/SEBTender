
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
findSebTenders();
 

//Get all  tenders from myProcurement
function findSebTenders(){
    //list of scrapped_tender object
    
    $scrapped_tenders = array();  
    
    $currenthtmlDoc = file_get_html("http://www2.sesco.com.my/etender/notice/notice.jsp");
    
    
    $htmlNodes = $currenthtmlDoc->find("//tbody/tr");
    $result = "";
    
        $rowcount = 0;
        
        foreach($htmlNodes as $trNode){
            if($rowcount > 0){
                //echo $trNode;
                $tdNodes = $trNode->find("td");
                $tdNodeCount = count($tdNodes);
                $count = 0;
                
                $scrappedTender = new scrapped_tender();
                //for each td nodes in one tr
                foreach($tdNodes as $tdNode){
                    if($tdNodeCount > 3){
                        
                    if(!IsNullOrEmptyString($tdNode)){
                        $innertdnode = $tdNode->innertext;
                        
                        
                        switch($count){
                            case 0:
                                $aNodeString = $tdNode->innertext;
                                echo "reference" . $aNodeString;
                                
                                break;

                            case 1:
                                echo "TITLE: " . $innertdnode . "<br/>";
                                //array_push($tender_title,$innertdnode);
                                //$scrappedTender->title = strip_tags($innertdnode);
                                //echo $scrappedTender->title;
                                break;

                            case 2:
                                echo "Originating Station: " . $innertdnode . "<br/>";
                                //array_push($tender_reference,$innertdnode);
                                //$scrappedTender->reference = strip_tags($innertdnode);
                                break;

                            case 3:
                                echo "Closing Date: " . $innertdnode . "<br/>";
                                //array_push($tender_category,$innertdnode);
                                //$scrappedTender->category = strip_tags($innertdnode);
                                break;

                            case 4:
                                echo "e-bidding closing date: " . $innertdnode . "<br/>";
                                //array_push($tender_ministry,$innertdnode);
                                //$scrappedTender->originatingSource = strip_tags($innertdnode);
                                break;

                            case 5:
                                echo "Document fee before gst: " . $innertdnode . "<br/>";
                                //array_push($tender_originator,$innertdnode);
                                //$scrappedTender->agency = strip_tags($innertdnode);
                                break;

                            case 6:
                                echo "gst: " . $innertdnode . "<br/>";
                                //array_push($tender_startingdate,$innertdnode);
                                //$scrappedTender->startDate = strip_tags($innertdnode);
                                break;

                            case 7:
                                echo "document gee after gst: " . $innertdnode . "<br/>";
                                //array_push($tender_closingdate,$innertdnode);
                                //$scrappedTender->closingDate = strip_tags($innertdnode);
                                break;
                        }
                        $count++;
                        //$currentTdCount++;
                    }
                }else{
                        if(!IsNullOrEmptyString($tdNode->find("td"))){
                            echo $tdNode->innertext;
                        }
                    }
                }
               
                $rowcount++;
            }else{
                $rowcount++;
            }
            
            
            

        }
        
        //$result = getNextPageLink($currenthtmlDoc);
        if($result == "No next page"){
            echo "hoi";
        }
        //$currenthtmlDoc = file_get_html("http://myprocurement.treasury.gov.my" . $result);
        //$htmlNodes = $currenthtmlDoc->find("//table/table/tr");
        
    
    
    //call function to insert scrapped tenders into database
    //insertIntoDatabase($scrapped_tenders);
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
                        (:reference, :title, :category, :originatingSource, 0, :agency, :startDate, :closingDate)");
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