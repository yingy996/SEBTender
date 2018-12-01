<!DOCTYPE html>
<html data-ng-app="">
<head>
    <title>Pocket Tender | View Survey List</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/stylesheet.css" rel="stylesheet" />
    
</head>
<body id="pocketTenderBody"> <!--Photo by rawpixel.com from Pexels -->
    <?php 
    include("header.php");
    include("process_viewSurveyList.php");
    
    ?>
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12" style="background-color:rgba(255, 255, 255, 0.7)">
                <div class="page-header">
                    <h3>Survey List</h3>
                </div>
                <?php
                if(count($result) > 0){
                    foreach($result as $key => $survey){
                        $surveyJson = json_encode($survey);
                        echo
                        '<div class="row contentRow">
                            <form id="selectsurvey" action="answerSurvey.php" method="post">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <p>Survey Title:<strong>
                                            '. $survey["surveyTitle"] . '
                                            </strong></p>
                                            <input type="hidden" name="surveytitleinput" value="' . $survey["surveyTitle"] .'">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <p>Survey Description: <strong>'. $survey["description"] . '</strong></p>
                                            <input type="hidden" name="surveyidinput" value="' . $survey["surveyID"] .'">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 text-right">
                                        <p>Closing Date: <strong>'.
                                            $survey["endDate"] . '</strong>
                                        </p>
                                        </div>
                                    </div>
                                    <p><input type="submit" class="btn btn-default" id="selectedsurveyBtn" value="selectsurvey"/></p>
                                </div>
                            </form>
                         </div>
                        ';             
                    }
                }else{
                    echo '<div class="col-xs-12">
                            <div class="row">
                                <p>No Surveys Available</p>
                            </div>
                          </div>';
                }
                    
                
                
                
                ?>
                <br/>
            </div>
        </div>
        
        <?php 
        include("footer.php");
        ?>
    </div>
    
</body>
</html>