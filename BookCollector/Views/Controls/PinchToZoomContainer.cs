// <copyright file="PinchToZoomContainer.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls;

public class PinchandPanContainer : ContentView
{
    private double currentScale = 1;
    private double startScale = 1;
    private double xOffset = 0;
    private double yOffset = 0;

    private double panX;
    private double panY;

    public PinchandPanContainer()
    {
        var pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += this.OnPinchUpdated;
        this.GestureRecognizers.Add(pinchGesture);

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += this.OnPanUpdated;
        this.GestureRecognizers.Add(panGesture);
    }

    public void OnPinchUpdated(object? sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            this.startScale = this.Content.Scale;
            this.Content.AnchorX = 0;
            this.Content.AnchorY = 0;
        }

        if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor to be applied.
            this.currentScale += (e.Scale - 1) * this.startScale;
            this.currentScale = Math.Max(1, this.currentScale);

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the X pixel coordinate.
            double renderedX = this.Content.X + this.xOffset;
            double deltaX = renderedX / this.Width;
            double deltaWidth = this.Width / (this.Content.Width * this.startScale);
            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the Y pixel coordinate.
            double renderedY = this.Content.Y + this.yOffset;
            double deltaY = renderedY / this.Height;
            double deltaHeight = this.Height / (this.Content.Height * this.startScale);
            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

            // Calculate the transformed element pixel coordinates.
            double targetX = this.xOffset - ((originX * this.Content.Width) * (this.currentScale - this.startScale));
            double targetY = this.yOffset - ((originY * this.Content.Height) * (this.currentScale - this.startScale));

            // Apply translation based on the change in origin.
            this.Content.TranslationX = Math.Clamp(targetX, -this.Content.Width * (this.currentScale - 1), 0);
            this.Content.TranslationY = Math.Clamp(targetY, -this.Content.Height * (this.currentScale - 1), 0);

            // Apply scale factor
            this.Content.Scale = this.currentScale;
        }

        if (e.Status == GestureStatus.Completed)
        {
            // Store the translation delta's of the wrapped user interface element.
            this.xOffset = this.Content.TranslationX;
            this.yOffset = this.Content.TranslationY;
        }
    }

    public void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                // Translate and pan.
                double boundsX = this.Content.Width;
                double boundsY = this.Content.Height;
                this.Content.TranslationX = Math.Clamp(this.panX + e.TotalX, -boundsX, boundsX);
                this.Content.TranslationY = Math.Clamp(this.panY + e.TotalY, -boundsY, boundsY);
                break;

            case GestureStatus.Completed:
                // Store the translation applied during the pan
                this.panX = this.Content.TranslationX;
                this.panY = this.Content.TranslationY;
                break;
        }
    }
}