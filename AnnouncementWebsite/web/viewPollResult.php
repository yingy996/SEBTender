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
        include("process_viewPollResult.php");

        ?>

        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <h1>View Poll Result</h1>
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
                                echo 'Error retreive poll question';
                                end;
                            }
                            ?>
                        </strong>
                    </p> <br/>
                    <p class="h4"><strong>Poll Response</strong></p>

                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <?php
                                // table header
                                echo '<th>Poll Option</th>
                                    <th>Response</th>';
                                ?>
                            </tr>
                        </thead>

                        <tbody>
                            <?php
                            // Display response data in table
                            $responseEmpty = true;

                            if (isset($options) && isset($response_answers))
                            {
                                if (count($response_answers) > 0) {

                                    foreach ($options as $option) {

                                        echo '<tr>';

                                        echo '<td>'. $option["optionTitle"] .'</td>';
                                        
                                        $i = 0;
                                        foreach ($response_answers as $response)
                                        {
                                            if ($option["optionTitle"] == $response["option"])
                                            {
                                                $i += 1;
                                            }
                                            
                                        }
                                        echo '<td>' . $i/$total_response*100 . '%</td>';
                                        $responseEmpty = false;

                                        echo '</tr>';
                                    }
                                } 
                            }

                            ?>
                        </tbody>

                    </table>

                    <?php

                    echo '<p><strong>
                        Number of respondents: ' . $total_response .
                        '</strong></p>';

                    if ($responseEmpty == true) 
                    {
                        echo '<p><strong>No respondents available.</strong></p> <br/>';
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