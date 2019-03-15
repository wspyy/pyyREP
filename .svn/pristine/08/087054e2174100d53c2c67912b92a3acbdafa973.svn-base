<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleTableImportExcel.aspx.cs"
    Inherits="SingleTable_SingleTableImportExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ImportData</title>
</head>
<body>
    <div class="row">
        <div class="col-md-12">
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-book"></i>导入数据</div>
                    <div class="tools">
                        <a class="collapse" href="javascript:;"></a><a class="reload" href="javascript:;">
                        </a><a class="remove" href="javascript:;"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <!-- BEGIN FORM-->
                    <form class="form-horizontal form-bordered form-row-stripped" id="formImport" runat="server"
                    enctype="multipart/form-data">
                    <div class="form-actions top ">
                        <button class="btn blue" onclick="ImportPage();" type="button">
                            导 入</button>
                        <button class="btn default" onclick="jumpReturn();" type="button">
                            返 回</button>&nbsp;
                    </div>
                    <div class="form-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-md-3">
                                        导入数据</label>
                                    <div class="col-md-9">
                                        <div class="fileupload fileupload-new" data-provides="fileupload">
                                            <div class="input-group">
                                                <span class="input-group-btn"><span class="uneditable-input"><i class="fa fa-file fileupload-exists">
                                                </i><span class="fileupload-preview"></span></span></span><span class="btn default btn-file">
                                                    <span class="fileupload-new"><i class="fa fa-paper-clip"></i>浏览...</span> <span class="fileupload-exists">
                                                        <i class="fa fa-undo"></i>选择</span>
                                                    <input type="file" class="default" id="fileUrl" name="fileUrl" />
                                                </span><a href="#" class="btn red fileupload-exists" data-dismiss="fileupload"><i
                                                    class="fa fa-trash-o"></i>移除</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-md-3">
                                        导入模板</label><div class="col-md-9">
                                            <a href="#" onclick="jumpExportDot();" class="form-control" style="color: Red; text-decoration: underline">
                                                点击下载</a>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </form>
                    <!-- END FORM-->
                </div>
            </div>
        </div>
    </div>
</body>
</html>
