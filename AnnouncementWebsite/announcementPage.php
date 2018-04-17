<!DOCTYPE html>

<html>
<head>
    <title>SESCo eTender Mobile Application Announcements</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/stylesheet.css" rel="stylesheet" />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <link rel="stylesheet" href="http://fortawesome.github.io/Font-Awesome/assets/font-awesome/css/font-awesome.css"> 
</head>


<body>
    <!-- Navigation Bar -->
    <?php 
		include("header.php");
        include("process_announcementPage.php");
    ?>
    
    <!-- Body content -->
    <div class="container-fluid">
        <!-- Sort and filter list -->
        <h1>Announcements</h1>
        <hr/>
        <?php 
			$index = 0;	
            
            
        
        if(count($results) > 0){
            foreach($results as $announcement){
                
                if($index == 0){
                    echo '<div class="row">';
                }
                $announcementID = $announcement["announcementID"];
                $index += 1;
                //check if edited before, if not then don't show the edited date
                if(!empty($announcement["editedBy"])){
                    $edited = 'Edited on: ' . $announcement["editedDate"] . ' By ' . $announcement["editedBy"];
                    
                } else{
                    $edited = "";
                }
                echo
                    '<div class="col-md-10 col-md-offset-1">
                        <div class="row" style="border:1px solid #cecece;">
                            <div class="col-xs-12">
                                <div class="col-md-12"><h3>' . $announcement["announcementTitle"] . '</h3>
                                </div>
                                <div class="col-md-3" style="padding-bottom:7px;">
                                <p style="font-size:10px">' . $announcement["publishedDate"] . ' By ' . $announcement["postedBy"] . '</p>
                                </div>
                                <div class="col-md-9 text-right" style="padding-bottom:7px;">
                                <p style="font-size:10px;">' . $edited . '</p>
                                </div>

                                <div class="col-md-12" style="padding-bottom:7px;">    
                                <p>' . $announcement["announcementContent"] . '</p>
                                </div>
                                
                                <div class="col-md-12 text-right" style="padding-bottom:7px;">
                                <form action="editAnnouncementPage.php" method="post">
                                    <input type="hidden" id ="edit_postID" name="edit_postID" value="' . $announcementID . '"/>
                                    <input type="submit" id="editButton" name="editButton" class="btn btn-success" value="Edit" style="font-size:10px"/>
                                        
                                    
                                </form>
                                </div>
                            </div>
                        </div>
                    </div>';
            }
        }
        
        ?>
        
            <!--<div class="col-md-10 col-md-offset-1">
                <div class="row" style="border:1px solid #cecece;">
                    <div class="col-xs-12">
                        <div class="col-md-12"><h3>Tidak Boleh Jual Dadah ehey</h3></div>
                        <div class="col-md-3" style="padding-bottom:7px;">
                            <p style="font-size:10px;">Yesterday at 2:15 PM</p>
                        </div>
                        <div class="col-md-9 text-right" style="padding-bottom:7px;">
                            <p style="font-size:10px;">Edited an hour ago at 2:15 PM by Ehey2</p>
                        </div>
                        
                        <div class="col-md-12" style="padding-bottom:7px;">    
                            <p>Penjualan dadah adalah dilarang sama sekale. Sesiapa yang didapati mejual dadah jenis pil khayal, Coccaine, Ubat Batuk, Panadol etc. akan di hukum penjara 1 hari dan 69 kali sebatan rotan ATAUPUN dipotong pelir.</p>
                        </div>
                        
                        
                        
                        </div>
                    </div>
                </div>-->
            
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
    <script src="js/angular.min.js"></script>
    <script src="js/app.js"></script>
</body>
</html>



