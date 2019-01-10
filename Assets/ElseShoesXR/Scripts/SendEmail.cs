using UnityEngine.UI;
using UnityEngine;
using System.Net;

public class SendEmail
{
    public static string to, subject, body, ipAddress;
    private static Text UX_Feedbacks;

    public static void Send()
    {
        if (to != null)
        {
            using (var message = new UT.MailMessage()
                .AddTo(to)
                .SetSubject(subject)
                .SetBody(body)
                .SetBodyHtml(true)
                )
                UT.Mail.Send(message, "authsmtp.register.it", "smtp@else-corp.it", "MEout_1237", false);
        }
        else
            return;
    }

    public static string DefineHtmlString(string imgURL)
    {
        string recipientName = "St.Gallen User";

        string currentTime = System.DateTime.Now.ToString();

        //string ipAddress = "";

        //foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        //    ipAddress += " || " + ip;

        string executionTime = Mathf.Ceil(Time.time).ToString() + " seconds.";

        string html = "<html>" +
            "<head>" +
                "<meta http-equiv=\"Content-Type" + "content=\"text/html; charset=windows-1252\">" +
                "<title>ELSE Order Confirmation [FAC-SIMILE]</title>" +
                "<link href = \"https://fonts.googleapis.com/css?family=Lato" + "rel=\"stylesheet\">" +
            "</head>" +
                "<body>" +
                    "<br>" +
                        "<table style = \"color: #1b1b1b; font-family: 'Lato', Arial, Helvetica, sans-serif; font-size: 12px; line-height: 1.5em;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"650\" align=\"center\">" +
                             "<tbody>" +
                                "<tr>" +
                                   "<td>" +
                                      "<table border = \"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">" +
                                         "<tbody>" +
                                            "<tr>" +
                                               "<td>" +
                                                  "<div style = \"background-color: #1b1b1b; height: 5px; margin-bottom: 0.5em;\" ></ div >" +
                                               "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                               "<td>" +
                                                  "<img src=" + imgURL + "/>" +
                                               "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                               "<td>" +
                                                  "<hr style = \"display: block; background-color: #1b1b1b; border: 0; height: 1px; margin-top: 1em;\">" +
                                               "</td>" +
                                            "</tr>" +
                                         "</tbody>" +
                                      "</table>" +
                                   "</td>" +
                                "</tr>" +
                                "<tr>" +
                                   "<td align=\"left\">" +
                                      "<table border = \"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">" +
                                         "<tbody>" +
                                            "<tr>" +
                                               "<td style = \"TEXT-ALIGN: left; FONT-FAMILY: Arial,Helvetica,sans-serif; COLOR: #1b1b1b; FONT-SIZE: 13px; line-height: 1.5em;\" >" +
                                                  "<div>" +
                                                     "<p style=\"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"> Dear " + recipientName + ", </span></p>" +
                                                     "<p style=\"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"> Here's your recent order confirmation:</span></p>" +
                                                  "</div>" +
                                                  "<br>" +
                                                  "<div>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Store: </span> <span> ELSE Shoes </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Order Date Time:</span> <span>  " + currentTime + "</span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> IP Address:</span> <span>  " + ipAddress + "</span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Execution Time:</span> <span>  " + executionTime + "</span> </p>" +
                                                  "</div>" +
                                                  "<br>" +
                                                  "<div>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> First Name: </span> <span> </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Last Name: </span> <span> </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Size: </span> <span>  </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\" ><span class=\"bold-text\" style=\"font-weight: bold;\"> Phone Number: </span> <span>  </span> </p>" +
                                                  "</div>" +
                                                  "<br>" +
                                                  "<div>" +
                                                     "<p class=\"bold-text\" style=\"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;font-weight: bold;\"> Product Configuration</p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Base Model: </span> <span>  </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Size: </span> <span>  </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Materials: </span> <span></span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Accessories: </span> <span>  </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Heel: </span> <span>  </span> </p>" +
                                                  "</div>" +
                                                  "<br>" +
                                                  "<div>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Total Price: </span> <span>  </span> &euro; </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> SKU: </span> <span> </span> </p>" +
                                                  "</div>" +
                                                  "<br>" +
                                                  "<div>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Delivery Address: </span> <span> </span> </p>" +
                                                     "<p style = \"display: block;-webkit-margin-before: 0.5em;-webkit-margin-after: 0.5em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;\"><span class=\"bold-text\" style=\"font-weight: bold;\"> Delivery Time: </span> <span> 21 </span> days</p>" +
                                                  "</div>" +
                                                  "<div class=\"button-group\" style=\"display: flex;flex-direction: row;\">" +
                                                     "<a href = \"{{url}}\">" +
                                                        "<div class=\"button\" style=\"display: flex;color: white;align-items: center;background-color: #ba3f3f;width: 150px;height: 30px;box-shadow: none;border: solid 1px white;margin: 5px 5px 5px 0px;cursor: pointer;justify-content: center;\"> Configuration Preview</div>" +
                                                      "</a>" +
                                                     "<a href = \"{{xml_link}}\" >" +

                                                         "<div class=\"button\" style=\"display: flex;color: white;align-items: center;background-color: #ba3f3f;width: 150px;height: 30px;box-shadow: none;border: solid 1px white;margin: 5px 5px 5px 0px;cursor: pointer;justify-content: center;\"> View XML</div>" +
                                                       "</a>" +
                                                  "</div>" +
                                               "</td>" +
                                            "</tr>" +
                                         "</tbody>" +
                                      "</table>" +
                                   "</td>" +
                                "</tr>" +
                                "<tr>" +
                                   "<td align = \"left\" >" +
                                      "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">" +
                                         "<tbody>" +
                                            "<tr>" +
                                               "<td align = \"left\" >" +
                                                  "<div style=\"FONT-FAMILY: Arial,Helvetica,sans-serif; font-size: 12px; line-height: 1.5em;\">" +
                                                     "<font color = \"#1b1b1b\" > Powered by E.L.S.E., in collaboration with ELSE Corp</font><br><br>" +
                                                     "</p>" +
                                                  "</div>" +
                                               "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                               "<td>" +
                                                  "<hr style = \"display: block; background-color: #1b1b1b; border: 0; height: 1px; margin-top: 0.5em; width: 650px;\" >" +
                                                "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                   "<td>" +
                                                      "<table width= \"100%\" cellspacing=\"0\" cellpadding=\"5\" border=\"0\">" +
                                                         "<tbody>" +
                                                            "<tr>" +
                                                               "<td align= \"left\" style= \"FONT-SIZE: 10px; padding-left: 5px ; FONT-FAMILY: Arial,Helvetica,sans-serif\" >© 2018 ELSE Corp. All Rights Reserved.</td>" +
                                                        "</tr>" +
                                                     "</tbody>" +
                                                  "</table>" +
                                               "</td>" +
                                            "</tr>" +
                                         "</tbody>" +
                                      "</table>" +
                                   "</td>" +
                                "</tr>" +
                             "</tbody>" +
                          "</table>" +
                       "</body>" +
                    "</html>";
        return html;
    }
}
