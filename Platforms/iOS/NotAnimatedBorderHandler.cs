namespace BorderHasBackgroundAnimation.Platforms.iOS;

using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Platform.ContentView;

public class NotAnimatedBorderHandler : BorderHandler
{
    private class BorderContentView : ContentView
    {
        public override void LayoutSubviews()
        {
            FindAnimationLayers(this.Layer);

            // #18204 workaround
            // This is the only workaround I found to avoid the animation when the border size is updated.
            // https://github.com/dotnet/maui/issues/15363
            // https://github.com/dotnet/maui/issues/18204
            if (this.Layer.Sublayers?.FirstOrDefault(layer => layer is MauiCALayer) is { AnimationKeys: not null } caLayer)
            {
                caLayer.RemoveAnimation("bounds");
                caLayer.RemoveAnimation("position");
            }

            base.LayoutSubviews();
        }

        public override void LayoutSublayersOfLayer(CALayer layer)
        {
            FindAnimationLayers(layer);
            base.LayoutSublayersOfLayer(layer);
        }

        public override NSObject ActionForLayer(CALayer layer, string eventKey)
        {
            FindAnimationLayers(layer);
            return base.ActionForLayer(layer, eventKey);
        }

        public override void DisplayLayer(CALayer layer)
        {
            FindAnimationLayers(layer);
            base.DisplayLayer(layer);
        }

        public override void DrawLayer(CALayer layer, CGContext context)
        {
            FindAnimationLayers(layer);
            base.DrawLayer(layer, context);
        }

        public override void WillDrawLayer(CALayer layer)
        {
            FindAnimationLayers(layer);
            base.WillDrawLayer(layer);
        }

        // Nothing found
        private static void FindAnimationLayers(CALayer layer)
        {
            if (layer.Sublayers is not null)
            {
                foreach (var innerLayer in layer.Sublayers)
                {
                    FindAnimationLayers(innerLayer);
                }
            }

            // This dosen't work
            layer.RemoveAllAnimations();

            if (layer.AnimationKeys is not null)
            {
                System.Diagnostics.Debug.WriteLine($"Animation keys found: ({string.Join(", ", layer.AnimationKeys)})");
                System.Diagnostics.Debug.WriteLine($"Class Name          : {layer.GetType().Name}");
            }
        }
    }

    protected override ContentView CreatePlatformView()
    {
        _ = this.VirtualView ?? throw new InvalidOperationException($"{nameof(this.VirtualView)} must be set to create a {nameof(ContentView)}");
        _ = this.MauiContext ?? throw new InvalidOperationException($"{nameof(this.MauiContext)} cannot be null");

        return new BorderContentView
        {
            CrossPlatformLayout = this.VirtualView
        };
    }

    // This also dosen't work
    public override void PlatformArrange(Rect rect)
    {
        // Disable the animation during arrange for the Border; otherwise, all resizing actions
        // will animate, and it makes the Border lag behind its content.

        CATransaction.Begin();
        CATransaction.AnimationDuration = 0;
        base.PlatformArrange(rect);
        CATransaction.Commit();
    }
}
