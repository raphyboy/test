Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.OleDb
Partial Class _Default
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim trow As New HtmlTableRow
    Dim tcell As New HtmlTableCell
    Dim table As New HtmlTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("code") <> "" Then
            SetOAuthSingleSignOn(Request.QueryString("code"))
            If Session("current_url") <> Nothing Then
                Response.Redirect(Session("current_url"))
            Else
                Response.Redirect("Default.aspx")
            End If

        ElseIf Session("token") = "" And Request.QueryString("token") = "" Then
            Response.Redirect("index.aspx")
        ElseIf Request.QueryString("token") <> "" Then
            Session("token") = Request.QueryString("token")
        End If
        If Not Page.IsPostBack Then
            If Request.QueryString("menu") IsNot Nothing Then
                Session("menu") = Request.QueryString("menu")
            End If
            Dim DT_List As DataTable
            Dim r As String = ""
            C.SetDropDownList(DropDownList1, "select * from CAPEX", "CAPEX_Name", "CAPEX_id")
            DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "' and Document_No is null ")
            GridView1.DataSource = DT_List
            GridView1.DataBind()
            'If DT_List.Rows.Count > 0 Then
            '    Dim i As Integer
            '    r = "<table id='table1'>"
            '    For i = 0 To DT_List.Rows.Count - 1
            '        r += "<tr>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("CAPEX_Name").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Asset_Type").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Cost").ToString + "</td>"
            '        r += "</tr>"
            '        'trow = New HtmlTableRow
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("CAPEX_Name").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Asset_Type").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Equipment_Cost").ToString
            '        'trow.Cells.Add(tcell)
            '        'table.Rows.Add(trow)
            '        'test.Controls.Add(table)
            '    Next
            '    r += "</table>"
            '    test.InnerHtml = r

            'End If
        End If
        'Label1.Text = test.InnerText

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DT As DataTable
        Dim DT_List As DataTable
        Dim strSql As String
        Dim r As String = ""
        DT = C.GetDataTable("select * from CAPEX where CAPEX_id='" + DropDownList1.SelectedValue + "'")

        If DT.Rows.Count > 0 Then
            strSql = "insert into List_CAPEX (CAPEX_Name, Asset_Type, Cost, CreateBy, CreateDate) values('" + DT.Rows(0).Item("CAPEX_Name").ToString + "','" + DT.Rows(0).Item("Asset_Type").ToString + "','" + DT.Rows(0).Item("Equipment_Cost").ToString + "','weraphon.r',getdate()) "
            C.ExecuteNonQuery(strSql)
            DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "'  and Document_No is null ")
            GridView1.DataSource = DT_List
            GridView1.DataBind()
            'If DT_List.Rows.Count > 0 Then
            '    Dim i As Integer
            '    r = "<table id='table1' runat='server'>"
            '    For i = 0 To DT_List.Rows.Count - 1
            '        r += "<tr>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("CAPEX_Name").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Asset_Type").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Cost").ToString + "</td>"
            '        r += "</tr>"
            '        'trow = New HtmlTableRow
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("CAPEX_Name").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Asset_Type").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Equipment_Cost").ToString
            '        'trow.Cells.Add(tcell)
            '        'table.Rows.Add(trow)
            '        'test.Controls.Add(table)
            '    Next
            '    r += "</table>"
            '    test.InnerHtml = r
            'End If

        End If


    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            'C.SetDropDownList(CType(e.Row.Cells(1).FindControl("ddlAssetType"), DropDownList), "select * from AssetType where status='1' order by Asset_order", "Asset_Type", "Asset_id")
            'CType(e.Row.Cells(1).FindControl("ddlAssetType"), DropDownList).SelectedValue = CType(e.Row.Cells(1).FindControl("lblID"), Label).Text
        End If
    End Sub

    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Dim id_list As String
        Dim strSql As String

        id_list = CType(GridView1.Rows(e.RowIndex).Cells(3).FindControl("lblID"), Label).Text
        strSql = "delete from List_CAPEX where id_List = '" + id_list + "' "
        C.ExecuteNonQuery(strSql)


        Dim DT_List As DataTable
        Dim r As String = ""
        C.SetDropDownList(DropDownList1, "select * from CAPEX", "CAPEX_Name", "CAPEX_id")
        DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "' and Document_No is null ")
        GridView1.DataSource = DT_List
        GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim strSql As String
        Dim i As Integer
        If GridView1.Rows.Count > 0 Then
            For i = 0 To GridView1.Rows.Count - 1
                strSql = "Update List_CAPEX Set CAPEX_Name='" + GridView1.Rows(i).Cells(0).Text + "', Asset_Type='" + CType(GridView1.Rows(i).Cells(1).FindControl("ddlAssetType"), DropDownList).SelectedValue + "', Cost='" + CType(GridView1.Rows(i).Cells(2).FindControl("TextBox3"), TextBox).Text + "', CreateBy='" + Session("uemail") + "', CreateDate = getdate() where id_List='" + CType(GridView1.Rows(i).Cells(3).FindControl("lblID"), Label).Text + "' "
                C.ExecuteNonQuery(strSql)
            Next
        End If
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("add_opex.aspx")
    End Sub
    Private Sub SetOAuthSingleSignOn(ByVal code As String)
        Dim DS As New DataSet
        Dim DT As New DataTable
        Dim HttpWReq As HttpWebRequest
        Dim httpWRes As HttpWebResponse = Nothing
        Dim address As Uri
        Dim strData As New StringBuilder
        Dim Client_id As String = "jcORmGdFNz_Feasi"
        Dim Client_Secret As String = "VclkIkOoqGBISJQaXnkf"
        Dim URI As String = "http://posweb.triplet.co.th/Feasibility/Default.aspx"

        address = New Uri("https://api.jasmine.com/authen1/oauth/token?client_id=" + Client_id + "&redirect_uri=" & URI & "&grant_type=authorization_code&code=" & code)
        HttpWReq = DirectCast(WebRequest.Create(address), HttpWebRequest)
        HttpWReq.Method = "POST"
        HttpWReq.ContentType = "application/x-www-form-urlencoded"

        SetBasicAuthHeader(HttpWReq, Client_id, Client_Secret)

        httpWRes = DirectCast(HttpWReq.GetResponse(), HttpWebResponse)

        Dim reader As StreamReader = New StreamReader(httpWRes.GetResponseStream())
        Dim json As String = reader.ReadToEnd()

        Dim vHeader() As String
        Dim Token() As String
        Dim Access_Token As String = ""
        vHeader = Split(json.ToString, ",")
        If vHeader.Length > 2 Then
            Token = Split(vHeader(0).ToString, ":")
            Access_Token = Replace(Token(1).ToString, """", "").ToString.Trim
        End If

        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse
        Request = DirectCast(WebRequest.Create(New Uri("https://api.jasmine.com/authen1/me")), HttpWebRequest)
        HttpWReq.Method = "GET"
        HttpWReq.ContentType = "application/x-www-form-urlencoded"
        Request.Headers("Authorization") = "Bearer " + Access_Token

        Session("token") = Access_Token

        Response = DirectCast(Request.GetResponse(), HttpWebResponse)
        reader = New StreamReader(Response.GetResponseStream())
        json = reader.ReadToEnd()

        DT = ConvertJSONToDataTable(json)
        Dim username As String() = DT.Rows(0).Item("username").ToString.Split("@")
        Session("uemail") = username(0)


    End Sub

    Public Sub SetBasicAuthHeader(ByVal request As WebRequest, ByVal userName As String, ByVal userPassword As String)
        Dim authInfo As String = userName + ":" + userPassword
        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo))
        request.Headers("Authorization") = "Basic " + authInfo
    End Sub

    Private Function ConvertJSONToDataTable(ByVal jsonString As String) As DataTable
        Dim dt As New DataTable
        'strip out bad characters
        Dim jsonParts As String() = jsonString.Replace("[{", "{").Replace("}]", "}").Split("},{")

        'hold column names
        Dim dtColumns As New List(Of String)

        'get columns
        For Each jp As String In jsonParts
            'only loop thru once to get column names
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each rowData As String In propData
                Try
                    If rowData.Split(":").Length - 1 <> 0 Then
                        Dim idx As Integer = rowData.IndexOf(":")
                        Dim n As String = rowData.Substring(0, idx - 1)
                        Dim v As String = rowData.Substring(idx + 1)
                        If Not dtColumns.Contains(n) Then
                            dtColumns.Add(n.Replace("""", ""))
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))
                End Try

            Next
            Exit For
        Next

        'build dt
        For Each c As String In dtColumns
            dt.Columns.Add(c)
        Next
        'get table data
        For Each jp As String In jsonParts
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim nr As DataRow = dt.NewRow
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                    Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                    nr(n) = v
                Catch ex As Exception
                    Continue For
                End Try

            Next
            dt.Rows.Add(nr)
        Next
        Return dt
    End Function

    Function CertificateValidationCallBack(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
End Class
