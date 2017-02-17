Imports System.IO
Imports System.Net
'DEVELOPER:SBTOPZZZ
Public Class Form1
    Dim catcher As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Text = "Processing..."
        Try
            'Create an FTP web request
            Dim ftpwebrequest As FtpWebRequest = DirectCast(WebRequest.Create(TextBox1.Text), FtpWebRequest)
            'Set properties
            With ftpwebrequest
                'ftp server username and password
                .Credentials = New NetworkCredential(TextBox2.Text, TextBox3.Text)
                'set the method to download
                If ComboBox1.SelectedIndex = 0 Then
                    .Method = WebRequestMethods.Ftp.ListDirectory
                ElseIf ComboBox1.SelectedIndex = 1 Then
                    .Method = WebRequestMethods.Ftp.ListDirectoryDetails
                End If
                'upload timeout to 100 seconds
                .Timeout = "100000"
            End With
            Dim ftpwebres As FtpWebResponse = CType(ftpwebrequest.GetResponse(), FtpWebResponse)
            Dim ftpstreamreader As StreamReader = New StreamReader(ftpwebres.GetResponseStream())
            'clear list of files
            ListBox1.Items.Clear()
            'start loading files from an FTP server into list
            While (ftpstreamreader.Peek() > -1)
                ListBox1.Items.Add(ftpstreamreader.ReadLine())
            End While
            ftpstreamreader.Close()
            Form2.Close()
            Me.Text = "user: " & TextBox2.Text & " - VirtualDrive"
            RichTextBox1.Text = "Successfully Logged In. Welcome " & TextBox2.Text & "."

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
            TextBox3.Text = ""
            ListBox1.Items.Clear()
            Form2.Close()
            RichTextBox1.Text = ex.Message
            Me.Text = "VirtualDrive"

        End Try
        Form2.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button1.PerformClick()
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            If System.IO.Path.GetExtension(ListBox1.SelectedItem.ToString) = "" Then
                Try
                    Dim se As String = ListBox1.SelectedItem.ToString
                    Dim ftpwebrequest As FtpWebRequest = DirectCast(WebRequest.Create(TextBox1.Text & "/" & ListBox1.SelectedItem.ToString & "/"), FtpWebRequest)
                    'Set properties
                    With ftpwebrequest
                        'ftp server username and password
                        .Credentials = New NetworkCredential(TextBox2.Text, TextBox3.Text)
                        'set the method to download
                        .Method = WebRequestMethods.Ftp.ListDirectory
                        'upload timeout to 100 seconds
                        .Timeout = "100000"
                    End With
                    Dim ftpwebres As FtpWebResponse = CType(ftpwebrequest.GetResponse(), FtpWebResponse)
                    Dim ftpstreamreader As StreamReader = New StreamReader(ftpwebres.GetResponseStream())
                    'clear list of files
                    ListBox1.Items.Clear()
                    'start loading files from an FTP server into list
                    While (ftpstreamreader.Peek() > -1)
                        ListBox1.Items.Add(ftpstreamreader.ReadLine())
                    End While
                    ftpstreamreader.Close()
                    Form2.Close()
                    RichTextBox1.Text = "Navigated to (" & se & ")."
                    catcher = se
                Catch ex As Exception
                    RichTextBox1.Text = ex.Message
                End Try
            Else

            End If
        ElseIf ComboBox1.SelectedIndex = 1 Then

        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.Host = "" Then

        Else
            TextBox1.Text = My.Settings.Host
        End If
        If My.Settings.User = "" Then

        Else
            TextBox2.Text = My.Settings.User
        End If
        ComboBox1.SelectedIndex = 0
        If TextBox1.Text = "" And TextBox2.Text = "" Then
            TextBox1.SelectAll()
        Else
            TextBox3.SelectAll()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.Host = TextBox1.Text
        My.Settings.User = TextBox2.Text
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            BackgroundWorker1.RunWorkerAsync()
        Else
            MsgBox("Uploading Cancelled.", MsgBoxStyle.Information, "User Cancelled")
            RichTextBox1.Text = "You cancelled Uploading of file."
        End If
    End Sub
    Private Sub Extra()
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.Text = "Uploading File (" & System.IO.Path.GetFileName(OpenFileDialog1.FileName) & ")"
            Try
                Dim mReq As System.Net.FtpWebRequest
                mReq = DirectCast(System.Net.WebRequest.Create(TextBox1.Text & "/" & OpenFileDialog1.SafeFileName.ToString), System.Net.FtpWebRequest)
                With mReq
                    .Credentials = New System.Net.NetworkCredential(TextBox2.Text, TextBox3.Text)
                    .Method = WebRequestMethods.Ftp.UploadFile
                    .Timeout = 100000
                    .UseBinary = True
                    .ContentLength = OpenFileDialog1.SafeFileName.Length
                End With
                Dim ufile() As Byte = File.ReadAllBytes(OpenFileDialog1.FileName.ToString)
                Dim ftpwebstreamrequest As Stream = mReq.GetRequestStream()
                ftpwebstreamrequest.Write(ufile, 0, ufile.Length)
                ftpwebstreamrequest.Close()
                ftpwebstreamrequest.Dispose()
                RichTextBox1.Text = "Successfully Uploaded Files. " & OpenFileDialog1.FileName
                MsgBox("File Successfully Uploaded to FTP-Drive.", MsgBoxStyle.Information, "Success")
                Button1.PerformClick()
            Catch ex As Exception
                RichTextBox1.Text = ex.Message
                Button1.PerformClick()
            End Try
        Else
            MsgBox("Uploading Cancelled.", MsgBoxStyle.Information, "User Cancelled")
            RichTextBox1.Text = "You cancelled Uploading of file."
            Button1.PerformClick()
        End If
        Me.Text = "user:" & TextBox2.Text & " - VirtualDrive"
        Button1.PerformClick()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ListBox1.Items.Count = 0 Then
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
        Else
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
            Button6.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If MsgBox("This will delete (" & ListBox1.SelectedItem.ToString & "), permanently from the FTP-Drive. Are you sure?", MsgBoxStyle.OkCancel, "Warning") = MsgBoxResult.Ok Then
            Try
                Dim mReq As System.Net.FtpWebRequest
                mReq = DirectCast(System.Net.WebRequest.Create(TextBox1.Text & "/" & ListBox1.SelectedItem.ToString), System.Net.FtpWebRequest)
                With mReq
                    .Credentials = New System.Net.NetworkCredential(TextBox2.Text, TextBox3.Text)
                    .Method = WebRequestMethods.Ftp.DeleteFile
                    .Timeout = 100000
                    .UseBinary = True
                End With
                Dim main As FtpWebResponse = mReq.GetResponse()
                Button2.PerformClick()
                RichTextBox1.Text = "You deleted (" & ListBox1.SelectedItem.ToString & ") from the FTP-Drive."
            Catch ex As Exception
                RichTextBox1.Text = ex.Message
            End Try
        Else
            Button2.PerformClick()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then

        Else
            Button1.PerformClick()
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If MsgBox("Do you want to download (" & ListBox1.SelectedItem.ToString & ") from the FTP-Drive?", MsgBoxStyle.OkCancel, "Download File") = MsgBoxResult.Ok Then
                Try
                    If ListBox1.Items.Count = 0 Then
                        MsgBox("No File Found!", MsgBoxStyle.Critical, "Please Click " & "GetFiles" & " for checksum.")
                        Me.Refresh()
                    Else
                        Try
                            Dim ftpwebrequest As FtpWebRequest = DirectCast(WebRequest.Create(TextBox1.Text & "/" & ListBox1.SelectedItem.ToString), FtpWebRequest)
                            'Set properties
                            With ftpwebrequest
                                'ftp server username and password
                                .Credentials = New NetworkCredential(TextBox2.Text, TextBox3.Text)
                                'set the method to download
                                .Method = WebRequestMethods.Ftp.DownloadFile
                                'upload timeout to 100 seconds
                                .Timeout = "100000"
                            End With

                            Dim ftpwebres As FtpWebResponse = CType(ftpwebrequest.GetResponse(), FtpWebResponse)
                            Dim ftpstream As Stream = ftpwebres.GetResponseStream()

                            With savefildlg
                                .Title = "Download " & ListBox1.SelectedItem.ToString & " To:"
                                .FileName = System.IO.Path.GetFileName(ftpwebrequest.RequestUri.LocalPath)
                                'set type of files
                            End With
                            If savefildlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                                Dim ftpfilestream As FileStream = File.Create(savefildlg.FileName)
                                Dim buff(10240) As Byte
                                Dim bytesread As Integer = 0
                                While True
                                    bytesread = ftpstream.Read(buff, 0, buff.Length)
                                    If (bytesread = 0) Then Exit While
                                    ftpfilestream.Write(buff, 0, bytesread)
                                End While
                                ftpfilestream.Close()
                                ftpstream.Close()
                            Else
                                Exit Sub
                            End If
                            MsgBox("(" & ListBox1.SelectedItem.ToString & ") Downloaded & Saved to (" & savefildlg.FileName)
                            RichTextBox1.Text = "(" & ListBox1.SelectedItem.ToString & ") Downloaded & Saved to (" & savefildlg.FileName & ")"
                        Catch ex As Exception
                            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
                            RichTextBox1.Text = ex.Message
                        End Try
                    End If
                Catch ex As Exception
                    RichTextBox1.Text = ex.Message
                End Try
            Else
                RichTextBox1.Text = "You cancelled to Download (" & ListBox1.SelectedItem.ToString & ") from the FTP-Drive."
            End If
        Catch ex As Exception
            RichTextBox1.Text = ex.Message
        End Try
        Button1.PerformClick()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form2.Show()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim mReq As System.Net.FtpWebRequest
            mReq = DirectCast(System.Net.WebRequest.Create(TextBox1.Text & "/" & OpenFileDialog1.SafeFileName.ToString), System.Net.FtpWebRequest)
            With mReq
                .Credentials = New System.Net.NetworkCredential(TextBox2.Text, TextBox3.Text)
                .Method = WebRequestMethods.Ftp.UploadFile
                .Timeout = 100000
                .UseBinary = True
                .ContentLength = OpenFileDialog1.SafeFileName.Length

            End With
            Dim ufile() As Byte = File.ReadAllBytes(OpenFileDialog1.FileName.ToString)
            Dim ftpwebstreamrequest As Stream = mReq.GetRequestStream()
            ftpwebstreamrequest.Write(ufile, 0, ufile.Length)
            ftpwebstreamrequest.Close()
            ftpwebstreamrequest.Dispose()
            RichTextBox1.Text = "Successfully Uploaded Files. " & OpenFileDialog1.FileName
            MsgBox("File Uploading Successful!", MsgBoxStyle.Information, "Success")
            Button1.PerformClick()
        Catch ex As Exception
            RichTextBox1.Text = ex.Message
            Button1.PerformClick()
        End Try
        Form3.Close()
        Button1.PerformClick()
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Form3.ProgressBar1.Value = e.ProgressPercentage
    End Sub
End Class
