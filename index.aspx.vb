
Partial Class index
    Inherits System.Web.UI.Page

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim client_id As String = "jcORmGdFNz_Feasi"
        Dim redirect_uri As String = "http://posweb.triplet.co.th/Feasibility/Default.aspx"

        Response.Redirect("https://api.jasmine.com/authen1/oauth/authorize?response_type=code&client_id=" & client_id & "&redirect_uri=" & redirect_uri)
    End Sub
End Class
