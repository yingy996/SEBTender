
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
require_once("dbcontroller.php");
$db_handle = new DBController();

//Check which information to return
//if(isset($_POST["searchReference"]) || isset($_POST["searchTitle"]) || isset($_POST["searchOriginatingSource"]) || isset($_POST["searchClosingDate"])) {
    /*$reference = $_POST["searchReference"];
    $title = $_POST["searchTitle"];
    $originatingSource = $_POST["searchOriginatingSource"];
    $closingDate = $_POST["searchClosingDate"];*/
    
    
    $initialQuery = "SELECT * FROM scrapped_tender WHERE";
    $laterQuery = "";
    $fullQuery = "";
    
    //TESTINGGG
    $reference = "";
    $title = "asdasd";
    $originatingSource = "asdsad";
    $closingDate = "asdasd";
    
    //TESTING ENDSSSS
    
    if($reference != ""){
        $laterQuery = $laterQuery . " reference = " . $reference;
    }
    if($title != ""){
        if($reference == ""){
            $laterQuery = $laterQuery . " title = " . $title;
        }else{
            $laterQuery = $laterQuery . " AND title = " . $title;
        }
    }
    if($originatingSource != ""){
        if($reference == "" && $title == ""){
            $laterQuery = $laterQuery . " originatingSource = " . $originatingSource;
        }else{
            $laterQuery = $laterQuery . " AND originatingSource = " . $originatingSource;
        }
    }
    if($closingDate != ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " closingDate = " . $closingDate;
        }else{
            
            $laterQuery = $laterQuery . " AND closingDate = " . $closingDate;
        }
    }
    
    $fullQuery = $initialQuery . $laterQuery;
    echo $fullQuery;
    
    /*if($reference != ""){
        $laterQuery = "reference = " . $reference;
        if($title != ""){
            $laterQuery = $laterQuery . " AND title = " . $title;
            if($originatingSource != ""){
                $laterQuery = $laterQuery . " AND originatingSource = " . $originatingSource;
                if($closingDate != ""){
                    $laterQuery = $laterQuery . " AND closingDate = " . $laterQuery;
                }
            }
        }
        
    }*/
    
        
        
        /*$query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE isEnded = 0");
        $query->execute();
        $result = $query->fetchAll();
        */
        
        /*if($result != null) {
            $resultString = json_encode($result);
            echo $resultString;
        } else {
            $error_message = "There is no poll for now.";
            if (isset($_SESSION["user_login"])) {
                $error_message += "Create a new poll?";
            }
            echo $error_message;
        }*/
    //}


/*function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}*/
?></label>
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