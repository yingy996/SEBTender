
<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>MBKS tenders</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg"> <!--full page background img -->
        <?php include("header.php"); ?>

        <div class="container container-fluid">
            <label>
                <?php
                require('simple_html_dom.php');
                require('tender_object.php');

                findMBKSTenders();

                //Get all  tenders from MBKS website
                function findMBKSTenders(){
                    $currenthtmlDoc = file_get_html("http://www.mbks.sarawak.gov.my/modules/web/pages.php?mod=webpage&sub=page&id=1382");
                    $htmlNodes = $currenthtmlDoc->find("//table/tbody/tr/td[@class='page_Content']/table/tbody/tr");

                    $mbks_domain = "http://www.mbks.gov.my";
                    /*
                    $reference = ""; 
                    $link = ""; 
                    $title = "";
                    $closingdate = "";*/

                    $rowCount = 0;
                    $mbksTenders = array();

                    foreach($htmlNodes as $trNode){
                        $tdNodes = $trNode->find("td");
                        $tdNodeCount = count($tdNodes);

                        // skip first row AND at least one row of tender
                        if($rowCount > 0 && $tdNodeCount > 1){

                            $currentTdCount = 0;
                            $tenderObject = new scrapped_tender();

                            foreach($tdNodes as $tdNode){
                                if(!IsNullOrEmptyString($tdNode->innertext)){
                                    $innertdnode = $tdNode->innertext;

                                    switch($currentTdCount){
                                        case 0:
                                            $num = $tdNode->innertext;
                                            $num = SanitizeString($num);
                                            echo $num . "<br/>";
                                            //array_push($tender_number,$tdNode);
                                            break;

                                        case 1:
                                            foreach ($tdNode->children as $childNode) 
                                            {
                                                foreach ($childNode->children as $innertag)
                                                {
                                                    foreach ($innertag->children as $inner2tag)
                                                    {
                                                        $ref = $inner2tag->innertext;
                                                        $ref = SanitizeString($ref);
                                                        echo "Reference: " . $ref . "<br/>"; 
                                                    }

                                                    if ($innertag->tag == "strong")
                                                    {
                                                        foreach ($innertag->children as $inner2tag)
                                                        {
                                                            $link = $inner2tag->href;
                                                            $link = SanitizeString($link);
                                                            echo "<br/>Document Link: " . $mbks_domain . $link . "<br/>";
                                                        }

                                                    } else {
                                                        $link = $innertag->href;
                                                        $link = SanitizeString($link);
                                                        echo "<br/>Document Link: " . $mbks_domain . $link . "<br/>";
                                                    }

                                                }
                                            }

                                            $tenderObject->reference = $ref;
                                            $tenderObject->fileLink = $mbks_domain . $link;
                                            //array_push($tender_reference,$ref);
                                            //array_push($tender_downloadlink,$link);
                                            break;

                                        case 2:
                                            foreach ($tdNode->children as $childNode) 
                                            {
                                                $title = $childNode->innertext;
                                                $title = SanitizeString($title);
                                                echo "Title: " . $title . "<br/>";
                                            }

                                            $tenderObject->title = $title; 
                                            //array_push($tender_description,$title);
                                            break;

                                        case 3:
                                            foreach ($tdNode->children as $childNode) 
                                            {
                                                foreach ($childNode->children as $innertag) 
                                                {
                                                    $closingDate = $innertag->innertext;
                                                    $closingDate = SanitizeString($closingDate);
                                                    echo "Closing Date: " . $closingDate . "<br/>";
                                                }
                                            }

                                            $tenderObject->closingDate = $closingDate;
                                            //array_push($tender_closingdate,$tdNode);
                                            break;

                                    }
                                    echo "<br/>";
                                    $currentTdCount++;
                                }
                            }

                            //Set tender source
                            $tenderObject->originatingSource = "MBKS";
                            $tenderObject->tenderSource = 3;

                            //Add the tender object to array
                            $mbksTenders[] = $tenderObject;

                            $rowCount++;
                        } else {
                            $rowCount++;
                        }
                        echo "<hr/>";
                    }

                    // Call function to insert scrapped tenders into database after finish looping
                    if (count($mbksTenders) > 0)
                    {
                        InsertIntoDatabase($mbksTenders);
                    }

                }

                // Insert scrapped tenders into database
                function InsertIntoDatabase($scrapped_tenders)
                {
                    require_once("dbcontroller.php");
                    $db_handle = new DBController();
                    $result = "";
                    $count = 0;

                    while(array_key_exists($count, $scrapped_tenders)){
                        $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, fileLinks, title, closingDate, originatingSource, tenderSource) VALUES
        (:reference, :fileLinks, :title, :closingDate, :originatingSource, :tenderSource)");
                        $query->bindParam(":reference", $scrapped_tenders[$count]->reference);
                        $query->bindParam(":fileLinks", $scrapped_tenders[$count]->fileLink);
                        $query->bindParam(":title", $scrapped_tenders[$count]->title);
                        $query->bindParam(":closingDate", $scrapped_tenders[$count]->closingDate);
                        $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
                        $query->bindParam(":tenderSource", $scrapped_tenders[$count]->tenderSource);

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
                function IsNullOrEmptyString($str)
                {
                    return (!isset($str) || trim($str) === '');
                }
                
                // Function for sanitizing string from html tag or encoding
                function SanitizeString($str)
                {
                    $str = html_entity_decode($str);
                    $str = strip_tags($str);
                    
                    return $str;
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