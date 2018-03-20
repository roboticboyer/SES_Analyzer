Imports System.IO
Imports System.Xml
Imports System.Console

Public Class Form1
    Public Dati_Ses(0 To 8, 0) As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim Alltext As String = ""
        Dim txt_riga As String
        Dim MyPos As Integer
        Dim txt_find As String = "TIME"
        Dim righe As Integer = -1
        Dim File_ses_Array(30) As String
        Dim streamtodisplay As StreamReader
        Dim r As Integer = 0
        Dim log_txt As String = ""
        Dim test_find As Boolean
        Dim txt_time As Double
        Dim txt_temp As String
        Dim righe_time_1(0) As Integer
        Dim righe_time_2(0) As Double
        Dim i_t As Integer = 0
        Dim num_righe_time As Integer

        Dim i_dati As Integer = 0
        Dim output_file As String
        Dim output_string As String
        Dim dato_ses As String
        Dim n_car As Integer


        ' Leggo file ses
        OpenFileDialog1.Filter = "SES file (*prn)|*.prn"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                streamtodisplay = My.Computer.FileSystem.OpenTextFileReader(OpenFileDialog1.FileName)
                'Cerco numero righe file
                Do Until streamtodisplay.EndOfStream
                    streamtodisplay.ReadLine()
                    righe = righe + 1
                Loop
                'lblNote.Text = righe
                ReDim File_ses_Array(righe)
                'Carico righe nell'array
                streamtodisplay = My.Computer.FileSystem.OpenTextFileReader(OpenFileDialog1.FileName)
                Do Until streamtodisplay.EndOfStream
                    File_ses_Array(r) = streamtodisplay.ReadLine
                    r = r + 1
                Loop
                'lblNote.Text = ((righe + 1) & " righe - Cartella: " & OpenFileDialog1.FileName)
                txtNote.Text = ((righe + 1) & " righe - Cartella: " & OpenFileDialog1.FileName)
                'txtNote.Text = Alltext
                'txtNote.Enabled
                streamtodisplay.Close()
                MsgBox("File caricato con successo")
            Catch ex As Exception
                MsgBox("E' sucesso un errore" & vbCrLf & ex.Message)
            End Try
        End If
        'Trovo righe iniziano con TIME
        For r = 0 To righe
            txt_riga = File_ses_Array(r)
            If txt_riga.Length > 21 Then
                txt_riga = txt_riga.Substring(0, 22)
                MyPos = InStr(txt_riga, txt_find, CompareMethod.Text)
                test_find = txt_riga.Contains("SECONDS")
                If (MyPos = 1 And test_find = True) Then
                    txt_temp = txt_riga.Substring(4, 10)
                    txt_temp = txt_temp.Trim
                    txt_time = Double.Parse(txt_temp, New System.Globalization.CultureInfo("en-US"))
                    log_txt = (log_txt & vbCrLf & (r + 1) & " - " & MyPos & " - " & txt_riga & " - " & txt_time.ToString)
                    ReDim Preserve righe_time_1(i_t)
                    ReDim Preserve righe_time_2(i_t)
                    righe_time_1(i_t) = r
                    righe_time_2(i_t) = txt_time
                    i_t = i_t + 1
                    'System.Console.WriteLine(log_txt)
                End If
            End If
        Next
        ReDim Preserve righe_time_1(i_t)
        righe_time_1(i_t) = righe
        'MyPos = InStr(4, Alltext, txt_find, CompareMethod.Text)
        'Console.OpenStandardOutput()
        'Console.Write(log_txt)
        Form2.Show()
        Form2.txt_note_F2.Text = log_txt
        MsgBox("Analisi terminata")
        num_righe_time = righe_time_1.Length

        'Trova dati Segmenti
        txt_find = "-  1"
        log_txt = ""
        For i_time = 0 To num_righe_time - 2
            For r = righe_time_1(i_time) To righe_time_1(i_time + 1)
                txt_riga = File_ses_Array(r)
                If txt_riga.Length > 99 Then
                    txt_temp = txt_riga.Substring(18, 4)
                    'MyPos = InStr(txt_temp, txt_find, CompareMethod.Text)
                    If txt_temp = txt_find Then

                        'Estraggo i dati
                        'Tempo
                        Dati_Ses(0, i_dati) = righe_time_2(i_time)
                        'Segmento
                        txt_temp = txt_riga.Substring(14, 3)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(1, i_dati) = txt_temp
                        'Calore sensibile
                        txt_temp = txt_riga.Substring(23, 13)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(2, i_dati) = txt_temp
                        'Calore latente
                        txt_temp = txt_riga.Substring(37, 13)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(3, i_dati) = txt_temp
                        'Temperatura
                        txt_temp = txt_riga.Substring(51, 15)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(4, i_dati) = txt_temp
                        'Umidità
                        txt_temp = txt_riga.Substring(67, 10)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(5, i_dati) = txt_temp
                        'Portata
                        txt_temp = txt_riga.Substring(78, 12)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(6, i_dati) = txt_temp
                        'Velocità
                        txt_temp = txt_riga.Substring(91, 9)
                        txt_temp = txt_temp.Trim
                        Dati_Ses(7, i_dati) = txt_temp
                        'Treno
                        If txt_riga.Length > 100 Then
                            n_car = txt_riga.Length - 101
                            txt_temp = txt_riga.Substring(101, n_car)
                            txt_temp = txt_temp.Trim
                            Dati_Ses(8, i_dati) = txt_temp
                        End If

                        i_dati = i_dati + 1
                        ReDim Preserve Dati_Ses(8, i_dati)


                        log_txt = (log_txt & vbCrLf & righe_time_2(i_time) & " - " & txt_riga)

                    End If
                End If
            Next
            log_txt = (log_txt & vbCrLf)
        Next
        'Console.Write(log_txt)
        Form2.txt_note_F2.Text = log_txt
        MsgBox("Analisi terminata")
        'Scrive file di output
        output_file = OpenFileDialog1.FileName
        output_file = output_file.Substring(0, output_file.Length - 3) & "csv"
        Dim file = My.Computer.FileSystem.OpenTextFileWriter(output_file, False)
        'Scrivo intestazione colonne
        output_string = "Tempo;Segmento;Calore sensibile;Calore Latente;Temperatura;Umidità;Portata;Velocità;Treno"
        file.WriteLine(output_string)
        'Scrivo dati nel file csv
        Dim num_dati = Dati_Ses.Length / 9
        For i = 0 To (num_dati - 2)
            output_string = ""
            For j = 0 To 8
                dato_ses = Dati_Ses(j, i)
                If dato_ses <> vbNullString Then
                    dato_ses = dato_ses.Replace(".", ",")
                Else
                    dato_ses = ""
                End If

                output_string = output_string & dato_ses & ";"
            Next
            file.WriteLine(output_string)
        Next

        file.Close()
        MsgBox("File CSV scritto")
        Form2.Hide()
        'Console.WriteLine("pippo")

        'Carico i dati nel listview
        Dim str(3) As String
        Dim itm As ListViewItem
        Dim num_elementi As Integer = 1
        ListView1.Items.Clear()
        For i = 0 To (num_dati - 2)

            str(0) = Dati_Ses(0, i)
            str(1) = Dati_Ses(1, i)
            str(2) = Dati_Ses(6, i)
            str(3) = Dati_Ses(7, i)
            itm = New ListViewItem(str)
            ListView1.Items.Add(itm)
            num_elementi = num_elementi + 1
        Next
        num_elementi = num_elementi - 1
        Text_elementi.Text = "Numero elementi = " & num_elementi.ToString

        'inserisce i tempi nel combobox
        ComboBox1.Show()
        Text_elementi.Show()
        Button3.Show()
        ComboBox1.Items.Clear()
        For i = 0 To (i_t - 1)
            ComboBox1.Items.Add(righe_time_2(i))

        Next

        'ListBox1.Items = righe_time_1
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'chiude aplicazione
        End
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedValueChanged
        'Seleziona un tempo
        'MsgBox(ComboBox1.SelectedItem)
        Dim tempo_sel As String = ComboBox1.SelectedItem
        'Carico i dati nel listview
        Dim str(3) As String
        Dim itm As ListViewItem
        Dim num_dati = Dati_Ses.Length / 9
        Dim num_elementi As Integer = 1
        ListView1.Items.Clear()
        For i = 0 To (num_dati - 2)
            If Dati_Ses(0, i) = tempo_sel Then
                str(0) = Dati_Ses(0, i)
                str(1) = Dati_Ses(1, i)
                str(2) = Dati_Ses(6, i)
                str(3) = Dati_Ses(7, i)
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
                num_elementi = num_elementi + 1
            End If
        Next
        num_elementi = num_elementi - 1
        Text_elementi.Text = "Numero elementi = " & num_elementi.ToString
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Carica tutti i dati nel listview
        Dim str(3) As String
        Dim itm As ListViewItem
        Dim num_dati = Dati_Ses.Length / 9
        Dim num_elementi As Integer = 1
        ListView1.Items.Clear()
        For i = 0 To (num_dati - 2)
            str(0) = Dati_Ses(0, i)
            str(1) = Dati_Ses(1, i)
            str(2) = Dati_Ses(6, i)
            str(3) = Dati_Ses(7, i)
            itm = New ListViewItem(str)
            ListView1.Items.Add(itm)
            num_elementi = num_elementi + 1
        Next
        num_elementi = num_elementi - 1
        Text_elementi.Text = "Numero elementi = " & num_elementi.ToString
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
