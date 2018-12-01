<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>Pocket Tender</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <link href="../css/stylesheet.css" rel="stylesheet" />

    </head>
    <body> <!--full page background img -->
        <?php 
        include("header.php");
        include("process_viewUserResponse.php");

        ?>

        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <h1>View Survey</h1>
                    <hr/>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <p class="h3"> 
                        <strong>
                            <?php
                            if (isset($survey)) {
                                foreach ($survey as $title)
                                    echo $title['surveyTitle'];
                            } else {
                                echo 'Error retrieve question title';
                                end;
                            }
                            ?>
                        </strong>
                    </p> 
                    <p class="h4">
                        <strong>Respondent ID: 
                            <?php
                            echo $_GET['userID'];
                            ?>
                        </strong>
                    </p>

                    <br/>

                    <?php
                    if (isset($questions)) {
                        foreach ($questions as $question) {
                            
                            $qNum=(int)$question["questionNumber"]+1;
                                
                            echo '<h4><strong>Question ' . $qNum . ' :</strong><em> ' . $question['questionTitle'] . '</em></h4><br/>';
                            
                            if (!empty($responses[$qNum-1][0])) {
                                echo '<p>' . $responses[$qNum-1][0] . '</p>';
                            } else {
                                //echo $responses[$qNum-1][1] . '<br/>';
                                echo '<p>' . $selection[$qNum-1][0] . '</p>';
                            }
                            echo '<hr/>';
                        }

                    } else {
                        echo 'Error retrieve question title';
                        end;
                    }
                    ?>


                </div>
            </div>

            <hr/>
        </div>   

        <!-- Footer -->
        <?php 
        include("footer.php");
        ?>

        <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
        <script src="../js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="../js/bootstrap.min.js"></script>
        <!--Basic AngularJS-->

    </body>
</html>