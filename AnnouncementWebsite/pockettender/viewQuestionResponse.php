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
        include("process_viewQuestionResponse.php");

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
                            if (isset($question)) {
                                foreach ($question as $title)
                                    echo $title[0];
                            } else {
                                echo 'Error retreive question title';
                                end;
                            }
                            ?>
                        </strong>
                    </p> <br/>
                    <p class="h4"><strong>Survey Question Response</strong></p>

                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <?php
                                if($_GET['questionType'] == 'longsentence')
                                {
                                    echo '<th style="width: 300px;">Response Answer</th>
                                    <th style="width: 80px;">Respondent ID</th>
                                    <th style="width: 80px;">Date Submitted</th>';
                                }
                                if($_GET['questionType'] == 'dropdown')
                                {
                                    echo '<th>Scale</th>
                                    <th>Response</th>';
                                }
                                ?>
                            </tr>
                        </thead>

                        <tbody>
                            <?php
                            // Display response data if questionType is longsentence
                            if($_GET['questionType'] == 'longsentence')
                            {
                                $responseEmpty = true;

                                if (count($response_answers) > 0) {

                                    foreach ($response_answers as $response) {

                                        echo '<tr>';

                                        echo '<td>'. $response["text_answer"] .'</td>
                                            <td>'. $response["userID"] .'</td>
                                            <td>'. $response["dateSubmitted"] .'</td>';

                                        $responseEmpty = false;

                                        echo '</tr>';
                                    }

                                }
                            }

                            // Display response data if questionType is dropdown
                            if($_GET['questionType'] == 'dropdown')
                            {
                                echo '<td>'. 1 .'</td>
                                        <td>'. $scale1/$total_response*100 . '%</td>';
                                echo '</tr>';
                                echo '<td>'. 2 .'</td>
                                        <td>'. $scale2/$total_response*100 . '%</td>';
                                echo '</tr>';
                                echo '<td>'. 3 .'</td>
                                        <td>'. $scale3/$total_response*100 . '%</td>';
                                echo '</tr>';
                                echo '<td>'. 4 .'</td>
                                        <td>'. $scale4/$total_response*100 . '%</td>';
                                echo '</tr>';
                                echo '<td>'. 5 .'</td>
                                        <td>'. $scale5/$total_response*100 . '%</td>';
                                echo '</tr>';

                            }
                            ?>
                        </tbody>

                    </table>

                    <?php
                    if($_GET['questionType'] == 'dropdown')
                    {
                        echo '<p><strong>
                        Number of respondents: ' . $total_response .
                        '</strong></p>';
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