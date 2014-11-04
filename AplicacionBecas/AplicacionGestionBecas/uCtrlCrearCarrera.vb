﻿Imports EntitiesLayer

Public Class uCtrlCrearCarrera

    Public Property mantenimientoCarreras As uCtrlMantenimientoCarreras
    Dim mBlnFormDragging As Boolean
    Dim listaDirectores As List(Of Usuario)
    Dim alerta As UctrlAlerta = New UctrlAlerta()

    Dim c As ColorDialog = New ColorDialog()
    ''' <summary>Metodo que se ejecuta cuando el usuario da click al boton seleccionar color, este metodo 
    ''' muestra al usuario una paleta de colores</summary>
    ''' <autor>Alvaro Artavia</autor>

    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click

        If c.ShowDialog() = DialogResult.OK Then
            btnColor.BackColor = c.Color
        End If
        btnColor.ForeColor = Color.White

    End Sub

    Private Sub uCtrlCrearCarrera_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove

        If mBlnFormDragging = True Then

            Dim position As Point = frmPrincipal.PointToClient(MousePosition)
            Me.Location = New Point(position)

        End If

    End Sub

    Private Sub uCtrlCrearCarrera_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp

        mBlnFormDragging = False
        Dim position As Point = frmPrincipal.PointToClient(MousePosition)
        Location = New Point(position)

    End Sub

    Public Sub uCtrlCrearCarrera_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

        mBlnFormDragging = True

    End Sub

    Public Sub llenarDirectoresCmb()

        Try

            listaDirectores = objGestorCarrera.consultarDirectoresAcademicos()

            For i As Integer = 0 To listaDirectores.Count - 1

                cmbAcademico.Items.Add(listaDirectores(i).primerNombre & " " & listaDirectores(i).primerApellido & " " & listaDirectores(i).segundoApellido)
            Next

        Catch ex As Exception

            alerta.lblAlerta.Text = ex.Message
            FrmIniciarSesion.principal.Controls.Add(alerta)
            alerta.BringToFront()
            alerta.Show()

        End Try

    End Sub

    ''' <summary>Metodo que se ejecuta cuando el usuario da click al boton añadir y llama a la clase
    ''' gestor para enviar la informaciòn</summary>
    ''' <autor>Alvaro Artavia</autor>

    Private Sub btnAñadir_Click(sender As Object, e As EventArgs) Handles btnAñadir.Click

        Dim nombre As String = txtNombre.Text
        Dim codigo As String = txtCodigo.Text
        Dim color As String = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") + c.Color.B.ToString("X2")
        Dim directorAcademico As String = cmbAcademico.Text
        Dim nombreCompleto As String
        Dim idDirector As String = ""

        For i As Integer = 0 To listaDirectores.Count - 1

            nombreCompleto = listaDirectores(i).primerNombre & " " & listaDirectores(i).primerApellido & " " &
            listaDirectores(i).segundoApellido

            If nombreCompleto.Equals(directorAcademico) Then

                idDirector = listaDirectores(i).identificacion

            End If

        Next

        Try

            objGestorCarrera.agregarCarrera(nombre, codigo, color, idDirector)
            objGestorCarrera.guardarCambios()
            mantenimientoCarreras.listarCarreras()

        Catch ex As Exception

            alerta.lblAlerta.Text = ex.Message
            FrmIniciarSesion.principal.Controls.Add(alerta)
            alerta.BringToFront()
            alerta.Show()
            Me.Dispose()

        End Try

    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Dispose()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Dispose()
    End Sub

    Private Sub uCtrlCrearCarrera_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        llenarDirectoresCmb()

    End Sub
End Class
