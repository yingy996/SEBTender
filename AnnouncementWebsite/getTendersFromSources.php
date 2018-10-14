<?php
require("simple_html_dom.php");
require("tender_object.php");
ini_set('max_execution_time', 0);
date_default_timezone_set("Asia/Kuching");
echo "Once<br/>";
deleteFromDatabase();
$tekelomTenders = extractTelekomTenders();
$mbksTenders = extractMBKSTenders();
//$sebTenders = extractSEBTenders();
$myProcurementTenders = extractMyProcurementTenders();

echo "Telekom: " . count($tekelomTenders) . "<br/>";
echo "MBKS: " . count($mbksTenders) . "<br/>";
//echo "SEB: " . count($sebTenders) . "<br/>";
echo "MyProcurement: " . count($myProcurementTenders) . "<br/>";
//insertIntoDatabase($sebTenders, 0);
insertIntoDatabase($myProcurementTenders, 1);
insertIntoDatabase($tekelomTenders, 2);
insertIntoDatabase($mbksTenders, 3);

//Get all  tenders from myProcurement
function extractMyProcurementTenders(){
    //list of scrapped_tender object
    
    $scrapped_tenders = array();  
    
    $currenthtmlDoc = file_get_html("http://myprocurement.treasury.gov.my/custom/p_iklan_tender.php");
    
    
    $htmlNodes = $currenthtmlDoc->find("//table/table/tr");
    $result = "";
    
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
    
    
    /*if (count($scrapped_tenders) > 0) {
        //delete all tenders from myProcurement website
        //deleteFromDatabase(1);
        //call function to insert scrapped tenders into database
        echo "My procrurement: " . count($scrapped_tenders);
        insertIntoDatabase($scrapped_tenders, 1);
    }*/
    return $scrapped_tenders;
}

//Get all  tenders from myProcurement
function extractTelekomTenders(){
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
                                            $telekomFileLinks = array();
                                            foreach ($tdNode->children as $childNode){
                                                if (trim($childNode) != "") {
                                                    if (isset($childNode->href)) {
                                                        $fileName = trim($childNode->innertext);
                                                        $link = $childNode->href;
                                                        
                                                        $telekomFileLinks[$fileName] = $link;
                                                        //echo "Link: " . $tdNode->innertext; 
                                                        //echo "Link: " . $link; 
                                                        
                                                    }
                                                }
                                            }
                                            $tenderObject->fileLink = json_encode($telekomFileLinks);
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
    /*if (count($telekomTenders) > 0) {
        //deleteFromDatabase(2);
        echo "Telekom: " . count($telekomTenders);
        insertIntoDatabase($telekomTenders, 2);
    }*/
    return $telekomTenders;
}

//Get all  tenders from MBKS website
function extractMBKSTenders(){
    $currenthtmlDoc = file_get_html("http://www.mbks.sarawak.gov.my/modules/web/pages.php?mod=webpage&sub=page&id=1382");
    $htmlNodes = $currenthtmlDoc->find("//table/tbody/tr/td[@class='page_Content']/table/tbody/tr");

    //$mbks_domain = "http://www.mbks.gov.my";
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
                            //echo $num . "<br/>";
                            //array_push($tender_number,$tdNode);
                            break;

                        case 1:
                            foreach ($tdNode->children as $childNode) 
                            {
                                foreach ($childNode->children as $innertag)
                                {
                                    $links = array();
                                    
                                    foreach ($innertag->children as $inner2tag)
                                    {
                                        $ref = $inner2tag->innertext;
                                        $ref = SanitizeString($ref);
                                        //echo "Reference: " . $ref . "<br/>"; 
                                    }

                                    if ($innertag->tag == "strong")
                                    {
                                        foreach ($innertag->children as $inner2tag)
                                        {
                                            $link = $inner2tag->href;
                                            $link = SanitizeString($link);
                                            //echo "<br/>Document Link: " . $link . "<br/>";
                                        }

                                    } else {
                                        $link = $innertag->href;
                                        $link = SanitizeString($link);
                                        //echo "<br/>Document Link: " . $link . "<br/>";
                                    }

                                    $links[$ref] = $link;
                                }
                            }

                            $tenderObject->reference = $ref;

                            $tenderObject->fileLink = json_encode($links);
                            //array_push($tender_reference,$ref);
                            //array_push($tender_downloadlink,$link);
                            break;

                        case 2:
                            // Use this loop when the tags are span -> strong 
                            foreach ($tdNode->children as $childNode) 
                            {
                                $title = $childNode->innertext;
                                $title = SanitizeString($title);
                            }

                            // Use this loop when the tags are span -> strong -> span
                            if (IsNullOrEmptyString($title))
                            {
                                // span
                                foreach ($tdNode->children as $childNode) 
                                {
                                    // strong
                                    foreach ($childNode->children as $innertag)
                                    {
                                        $title = $innertag->innertext;
                                        $title = SanitizeString($title);
                                    } 
                                }
                            }

                            $title = SanitizeString($title);
                            //echo "Title: " . $title . "<br/>";
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
                                }
                            }

                            //echo "Closing Date: " . $closingDate . "<br/>";
                            $tenderObject->closingDate = $closingDate;
                            //array_push($tender_closingdate,$tdNode);
                            break;

                    }
                    //echo "<br/>";
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
        //echo "<hr/>";
    }

    // Call function to insert scrapped tenders into database after finish looping
    /*if (count($mbksTenders) > 0)
    {
        echo "MBKS: " . count($mbksTenders);
        insertIntoDatabase($mbksTenders, 3);
    }*/
    return $mbksTenders;

}

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

//Check if next page exists in myProcurement
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
function insertIntoDatabase($scrapped_tenders, $tenderSource){
    require_once("dbcontroller.php");
    $db_handle = new DBController();
    $result = "";
    $count = 0;
    while(array_key_exists($count, $scrapped_tenders)){
        if ($tenderSource == 0) { //Sarawak Energy SEB
            $query = $db_handle->getConn()->prepare("INSERT INTO scrapped_tender (reference, title, originatingSource, tenderSource, closingDate, startDate, docInfoJson, originatorJson, fileLinks) VALUES
            (:reference, :title, :originatingSource, :tenderSource, STR_TO_DATE(:closingDate, '%d-%m-%Y'), STR_TO_DATE(:startDate, '%d-%m-%Y'), :docInfoJson, :originatorJson, :fileLinks)");
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
            (:reference, :title, :category, :originatingSource, 1, :agency, STR_TO_DATE(:closingDate, '%d/%m/%Y'), STR_TO_DATE(:startDate, '%d/%m/%Y'))");
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
            (:title, :originatingSource, :tenderSource, STR_TO_DATE(:startDate, '%d/%m/%Y'), :fileLinks)");
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

//Function to delete the tenders from a selected source
function deleteFromDatabase() {
    require_once("dbcontroller.php");
    $db_handle = new DBController();
    $result = "";
    
    try {
        $query = $db_handle->getConn()->prepare("TRUNCATE TABLE scrapped_tender");
        $result = $query->execute();
    } catch (PDOException $ex) {
        echo "Delete Error: " . $ex;
    }
    
    //do {
        //Delete all the tenders from a selected source
        //$query = $db_handle->getConn()->prepare("DELETE FROM scrapped_tender WHERE tenderSource = :tenderSource");
        //$query->bindParam(":tenderSource", $tenderSource);
        //$query = $db_handle->getConn()->prepare("TRUNCATE TABLE scrapped_tender");
        //$result = $query->execute();
        //echo $result;
    //} while ($result == false);
}

// Function for basic field validation (present and neither empty nor only white space
function IsNullOrEmptyString($str){
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