<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInformation.aspx.cs"
    Inherits="Framework_RightsManagement_UserInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UserInfoManage</title>
</head>
<body>
    <div class="row-full">
        <div class="col-md-12">
            <div class="portlet box blue">
                <div class="portlet-title">
                    <%=strManageTitle%>
                    <div class="tools">
                        <a href="javascript:;" class="collapse"></a><a href="javascript:;" class="reload"
                            onclick="refreshForm();"></a><a href="javascript:;" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <!-- BEGIN FORM-->
                    <form class="form-horizontal form-bordered form-row-stripped" id="formManage" runat="server"
                        enctype="multipart/form-data">
                        <%=strManageButton%>
                        <%=strManageBody%>
                    </form>
                    <!-- END FORM-->
                </div>
            </div>
        </div>
    </div>
</body>
</html>
