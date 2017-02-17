Imports System.Net
Imports System.IO

Public Class Form2

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("Directory name cannot be (null). Please enter a name for the Directory.", MsgBoxStyle.Critical, "Directory cannot be (null)")
        Else
            Try
                Dim mReq As System.Net.FtpWebRequest
                mReq = DirectCast(System.Net.WebRequest.Create(Form1.TextBox1.Text & "/" & TextBox1.Text & "/"), System.Net.FtpWebRequest)
                With mReq
                    .Credentials = New System.Net.NetworkCredential(Form1.TextBox2.Text, Form1.TextBox3.Text)
                    .Method = WebRequestMethods.Ftp.MakeDirectory
                    .Timeout = 100000
                    .UseBinary = True
                End With
                Dim main As FtpWebResponse = mReq.GetResponse()
                MsgBox("Directory Created! (" & TextBox1.Text & ")")
                Form1.Show()
                Close()
                Form1.RichTextBox1.Text = "You created a Directory. (" & TextBox1.Text & ")"
                init()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
                Form1.Show()
                Close()
                Form1.RichTextBox1.Text = ex.Message
            End Try
        End If
    End Sub
    Private Sub init()
        Try
            Dim se As String = TextBox1.Text
            Dim ftpwebrequest As FtpWebRequest = DirectCast(WebRequest.Create(Form1.TextBox1.Text & "/" & TextBox1.Text & "/"), FtpWebRequest)
            'Set properties
            With ftpwebrequest
                'ftp server username and password
                .Credentials = New NetworkCredential(Form1.TextBox2.Text, Form1.TextBox3.Text)
                'set the method to download
                .Method = WebRequestMethods.Ftp.ListDirectory
                'upload timeout to 100 seconds
                .Timeout = "100000"
            End With
            Dim ftpwebres As FtpWebResponse = CType(ftpwebrequest.GetResponse(), FtpWebResponse)
            Dim ftpstreamreader As StreamReader = New StreamReader(ftpwebres.GetResponseStream())
            'clear list of files
            Form1.ListBox1.Items.Clear()
            'start loading files from an FTP server into list
            While (ftpstreamreader.Peek() > -1)
                Form1.ListBox1.Items.Add(ftpstreamreader.ReadLine())
            End While
            ftpstreamreader.Close()
        Catch ex As Exception
            Form1.RichTextBox1.Text = ex.Message
        End Try
    End Sub
End Class