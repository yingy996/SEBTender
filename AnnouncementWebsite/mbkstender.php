
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

                findMBKSTenders();

                //Get all  tenders from MBKS website
                function findMBKSTenders(){
                    $currenthtmlDoc = file_get_html("http://www.mbks.sarawak.gov.my/modules/web/pages.php?mod=webpage&sub=page&id=1382");
                    $htmlNodes = $currenthtmlDoc->find("//table/tbody/tr/td[@class='page_Content']/table/tbody/tr");
                    $result = "";

                    $tender_number = array();
                    $tender_reference = array();
                    //$tender_link = array();
                    $tender_description = array();
                    $tender_closingdate = array();


                        /*require_once("dbcontroller.php");
                    $db_handle = new DBController();

                    

                    $query = $db_handle->getConn()->prepare("INSERT INTO myprocurementtender (tender_number, tender_title, tender_reference, tender_category, tender_ministry, tender_originator, tender_startingdate, tender_closingdate) VALUES
                    (:randomID, :announcement_title, :announcement_content, NOW(), NULL, NULL, :login_user)");
                    $query->bindParam(":randomID", $randomID);
                    $query->bindParam(":announcement_title", $announcement_title);
                    $query->bindParam(":announcement_content", $announcement_content);
                    $query->bindParam(":login_user", $login_user);

                    $result = $query->execute();*/


                        $count = 0;

                        foreach($htmlNodes as $trNode){
                            $tdNodes = $trNode->find("td");
                            $tdNodeCount = count($tdNodes);

                            // skip first row
                            if($count > 0){

                                $currentTdCount = 0;

                                foreach($tdNodes as $tdNode){
                                    if(!IsNullOrEmptyString($tdNode->innertext)){
                                        $innertdnode = $tdNode->innertext;

                                        switch($currentTdCount){
                                            case 0:
                                                echo "Number: " . $innertdnode . "<br/>";
                                                array_push($tender_number,$innertdnode);
                                                break;

                                            case 1:
                                                echo "Reference: " . $innertdnode . "<br/>";
                                                array_push($tender_reference,$innertdnode);
                                                break;

                                            case 2:
                                                echo "Title: " . $innertdnode . "<br/>";
                                                array_push($tender_description,$innertdnode);
                                                break;

                                            case 3:
                                                echo "Closing Date: " . $innertdnode . "<br/>";
                                                array_push($tender_closingdate,$innertdnode);
                                                break;

                                        }
                                        echo "<br/>";
                                        $currentTdCount++;
                                    }
                                }
                                $count++;
                            } else {
                                $count++;
                            }
                            echo "<hr/>";

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