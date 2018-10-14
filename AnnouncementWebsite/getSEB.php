<?php
require("simple_html_dom.php");
require("tender_object.php");
ini_set('max_execution_time', 0);
date_default_timezone_set("Asia/Kuching");

$sebTenders = extractSEBTenders();
echo "SEB: " . count($sebTenders) . "<br/>";
insertIntoDatabase($sebTenders, 0);

//Extract tenders from Sarawak Energy e-Tender website
function extractSEBTenders() {
    $currenthtmlDoc = file_get_html("http://www2.sesco.com.my/etender/notice/notice.jsp");
    $htmlNodes = $currenthtmlDoc->find("//tbody/tr");
    $result = "";
    $sebTenders = array(); //array to store the tender objects
    
    while ($result != "No next page") {      
        $rowCount = 0;
        if ($htmlNodes != null) {
            foreach ($htmlNodes as $trNode) {
                //If the row is not the first row, create the tender item
                if ($rowCount > 27) {               
                    //Get contact details of the tender 
                    $tdNodes = $trNode->children();
                    $tdNodeCount = count($tdNodes);
                    $count = 0;
                    //echo $tdNodeCount;

                    if ($tdNodeCount > 7) {
                        $tenderObject = new scrapped_tender();
                        $tenderDoc = new tenderDocInfo();
                        $originatorInfo = new originatorInfo();
                        //echo "------ tender ". $rowCount ." -------";
                        //echo "<br/>";
                        foreach ($tdNodes as $tdNode) {
                            //echo $tdNode->innertext;
                            if ($tdNodeCount > 3) {
                                if (!IsNullOrEmptyString($tdNode->innertext)) {

                                    switch ($count) {
                                        case 0:
                                            foreach ($tdNode->children() as $tdChildNode) {
                                                if (isset($tdChildNode->href)) {
                                                    //echo "<strong>Reference:</strong> " . $tdChildNode->innertext;
                                                    //echo "<br/>";

                                                    $tenderObject->reference = $tdChildNode->innertext;
                                                }
                                            }
                                            break;
                                        case 1:
                                            //$tenderObject->title = $tdNode->first_child()->innertext;
                                            /*
                                            if ($tdNode->first_child() != null) {
                                                $title = trim($tdNode->first_child()->innertext);
                                                echo "CHILD: " . $title;
                                            } else {
                                                echo "null";
                                            }

                                            echo "<br/>";*/
                                            $tenderObject->title = $tdNode->first_child()->innertext;
                                            //echo "<strong>Title:</strong> " . $tdNode->first_child()->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 2:
                                            $tenderObject->originatingSource = "Sarawak Energy " . $tdNode->innertext;
                                            //echo "<strong>Originating Station:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 3:
                                            $tenderObject->closingDate = $tdNode->innertext;
                                            //echo "<strong>Closing Date:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 4:
                                            $tenderDoc->bidCloseDate = $tdNode->innertext;
                                            //echo "<strong>Bid closing date:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 5:
                                            $tenderDoc->feeBeforeGST = $tdNode->innertext;
                                            //echo "<strong>Fee before GST:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 6:
                                            $tenderDoc->feeGST = $tdNode->innertext;
                                            //echo "<strong>Fee GST:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                        case 7:
                                            $tenderDoc->feeAfterGST = $tdNode->innertext;
                                            //echo "<strong>Fee AFTER GST:</strong> " . $tdNode->innertext;
                                            //echo "<br/>";
                                            break;
                                    }
                                    $count++;
                                }
                            } else {
                                if (!IsNullOrEmptyString($tdNode->innertext)) {
                                    $tenderObject->tendererClass = $tdNode->innertext;
                                }
                            }
                        }

                        if ($tdNodeCount > 7) {
                            //Get the ORIGINATOR details of the tender item
                            $url = "http://www2.sesco.com.my/etender/notice/notice_originator.jsp?Referno=" . urlencode($tenderObject->reference);

                            $originatorHtml = file_get_html($url);

                            $originatorTrNodes = $originatorHtml->find("//table/tr/td/table/tr");
                            $originatorTrRowCount = 0;

                            if ($originatorTrNodes != null) {
                                foreach ($originatorTrNodes as $originatorTrNode) {
                                    $originatorTdNodes = $originatorTrNode->children;

                                    foreach ($originatorTdNodes as $originatorTdNode) {
                                        if (!IsNullOrEmptyString($originatorTdNode->innertext)) {
                                            if ($originatorTdNode->first_child() == null) {
                                                //The originator info starts on row 3, thus row 0, 1, 2 are skipped
                                                switch ($originatorTrRowCount) {
                                                    case 3:
                                                        $originatorInfo->name = $originatorTdNode->innertext;
                                                        break;
                                                    case 4:
                                                        $originatorInfo->officePhone = $originatorTdNode->innertext;
                                                        break;
                                                    case 5:
                                                        $originatorInfo->extension = $originatorTdNode->innertext;
                                                        break;
                                                    case 6:
                                                        $originatorInfo->mobilePhone = $originatorTdNode->innertext;
                                                        break;
                                                    case 7:
                                                        $originatorInfo->email = $originatorTdNode->innertext;
                                                        break;
                                                    case 8:
                                                        $originatorInfo->fax = $originatorTdNode->innertext;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                        }
                                    }
                                    $originatorTrRowCount++;
                                }
                            }

                            //Get the downloadable files of the tender item
                            $url2 = "http://www2.sesco.com.my/etender/notice/notice_tender.jsp?Referno=" . urlencode($tenderObject->reference);

                            $downloadLinkHtml = file_get_html($url2);
                            $filesTdNodes = $downloadLinkHtml->find("//table/tr/td/a");
                            $fileLinks = array();

                            if ($filesTdNodes != null) {
                                foreach ($filesTdNodes as $fileChildNode) {
                                    if (trim($fileChildNode->innertext) != ""){
                                        //echo "File: " . $fileChildNode->outertext . "<br/>";
                                        $linkSplit = explode("noticeDoc/", $fileChildNode->href);
                                        $fileLink = "http://www2.sesco.com.my/noticeDoc/" . $linkSplit[1];
                                        $fileLinks[$fileChildNode->innertext] = $fileLink;
                                    }
                                }
                            }
                        }

                        $tenderObject->docInfoJson = json_encode($tenderDoc);
                        $tenderObject->originatorJson = json_encode($originatorInfo);
                        if (count($fileLinks) > 0){
                            $tenderObject->fileLink = json_encode($fileLinks);
                        } else {
                            $tenderObject->fileLink = null;
                        }
                        
                        $tenderObject->tenderSource = 0;
                        //echo "<strong>Doc Info:</strong> ". $tenderObject->docInfoJson . "<br/>";
                        //echo "<strong>Originator Info:</strong> " . $tenderObject->originatorJson . "<br/>";
                        //echo "<strong>File links:</strong>" . $tenderObject->fileLink . "<br/>";

                        $sebTenders[] = $tenderObject;
                        //echo "<br/><hr/><br/>";
                    }
                }
                $rowCount++;
            }
        }
        
        $result = getSEBNextPage($currenthtmlDoc);
        /*if ($result != "No next page") {
            $currenthtmlDoc = file_get_html($result);
            $htmlNodes = $currenthtmlDoc->find("//tbody/tr");
        }*/
        if ($result == "No next page") {
            break;
        }
        $currenthtmlDoc = file_get_html($result);
        $htmlNodes = $currenthtmlDoc->find("//tbody/tr");
    }
    
    if (count($sebTenders) > 0) {
        //Insert into database
        //echo "SEB: " . count($sebTenders);
        
        //insertIntoDatabase($sebTenders, 0);
    }
    return $sebTenders;
}

function getSEBNextPage($htmlDoc) {
    $aNodes = $htmlDoc->find("//a");
    $result = "No next page";
    if ($aNodes != null) {
        foreach ($aNodes as $aNode) {
            if ($aNode->innertext == "Next") {
                $result = "http://www2.sesco.com.my/etender/notice/" . $aNode->href;
            }
        }
    }
    return $result;
}

//function to loop through list of tender items in $scrapped_tenders and insert into database
function insertIntoDatabase($scrapped_tenders, $tenderSource){
    require_once("dbcontroller.php");
    $db_handle = new DBController();
    $result = "";
    $count = 0;
    while(array_key_exists($count, $scrapped_tenders)){
        if ($tenderSource == 0) { //Sarawak Energy SEB
            $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, title, originatingSource, tenderSource, closingDate, startDate, docInfoJson, originatorJson, fileLinks) VALUES
            (:reference, :title, :originatingSource, :tenderSource, STR_TO_DATE(:closingDate, '%d-%m-%y'), STR_TO_DATE(:startDate, '%d-%m-%y'), :docInfoJson, :originatorJson, :fileLinks)");
            $query->bindParam(":reference", $scrapped_tenders[$count]->reference);
            $query->bindParam(":title", $scrapped_tenders[$count]->title);
            $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
            $query->bindParam(":tenderSource", $scrapped_tenders[$count]->tenderSource);
            //extract only date from the date and time string
            $SEBstartingDateQuery = strtok($scrapped_tenders[$count]->startDate, "");
            $SEBclosingDateQuery = strtok($scrapped_tenders[$count]->closingDate, "");
            $query->bindParam(":closingDate", $SEBclosingDateQuery);
            $query->bindParam(":startDate", $SEBstartingDateQuery);
            $query->bindParam(":docInfoJson", $scrapped_tenders[$count]->docInfoJson);
            $query->bindParam(":originatorJson", $scrapped_tenders[$count]->originatorJson);
            $query->bindParam(":fileLinks", $scrapped_tenders[$count]->fileLink);
        } elseif ($tenderSource == 1) { //myProcurement
            $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, title, category, originatingSource, tenderSource, agency, closingDate, startDate) VALUES
            (:reference, :title, :category, :originatingSource, 1, :agency, STR_TO_DATE(:closingDate, '%d/%m/%y'), STR_TO_DATE(:startDate, '%d/%m/%y'))");
            $query->bindParam(":reference", $scrapped_tenders[$count]->reference);
            $query->bindParam(":title", $scrapped_tenders[$count]->title);
            $query->bindParam(":category", $scrapped_tenders[$count]->category);
            $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
            //$query->bindParam(":tenderSource", "0");
            $query->bindParam(":agency", $scrapped_tenders[$count]->agency);
            $query->bindParam(":startDate", $scrapped_tenders[$count]->startDate);
            $query->bindParam(":closingDate", $scrapped_tenders[$count]->closingDate);
        } elseif ($tenderSource == 2){ //Telekom
            $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (title, originatingSource, tenderSource, startDate, fileLinks) VALUES
            (:title, :originatingSource, :tenderSource, STR_TO_DATE(:startDate, '%d/%m/%y'), :fileLinks)");
            $query->bindParam(":title", $scrapped_tenders[$count]->title);
            $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
            $query->bindParam(":tenderSource", $tenderSource);
            $query->bindParam(":startDate", $scrapped_tenders[$count]->startDate);
            $query->bindParam(":fileLinks", $scrapped_tenders[$count]->fileLink);
        } elseif ($tenderSource == 3) { //MBKS
            $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, fileLinks, title, closingDate, originatingSource, tenderSource) VALUES
            (:reference, :fileLinks, :title, STR_TO_DATE(:closingDate, '%d %M %Y'), :originatingSource, :tenderSource)");
            $query->bindParam(":reference", $scrapped_tenders[$count]->reference);
            $query->bindParam(":fileLinks", $scrapped_tenders[$count]->fileLink);
            $query->bindParam(":title", $scrapped_tenders[$count]->title);
            $query->bindParam(":closingDate", $scrapped_tenders[$count]->closingDate);
            $query->bindParam(":originatingSource", $scrapped_tenders[$count]->originatingSource);
            $query->bindParam(":tenderSource", $scrapped_tenders[$count]->tenderSource);
        }
        
        $result = $query->execute();
        if($result == true){
            $count++;
        }
    }
    
    if ($result) {
        //echo "Tenders have been successfully stored!";
        //Update the "last updated date" in tender source table
        $date = date("Y-m-d G:i:s");
        $updateQuery = $db_handle->getConn()->prepare("UPDATE tender_source SET lastUpdate = :time WHERE id = :id");
        $updateQuery->bindParam(":id", $tenderSource);
        $updateQuery->bindParam(":time", $date);
        $updateQuery->execute();
    }
}

// Function for basic field validation (present and neither empty nor only white space
function IsNullOrEmptyString($str){
    return (!isset($str) || trim($str) === '');
}
?>