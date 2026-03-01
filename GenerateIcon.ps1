# GenerateIcon.ps1
# Draws a weather-themed icon (sun + cloud + raindrops) and saves
# WeatherApp/WeatherApp.ico containing 16x16, 32x32, 48x48, 64x64, 256x256 images.

Add-Type -AssemblyName System.Drawing

function New-WeatherBitmap {
    param([int]$S)

    $bmp = New-Object System.Drawing.Bitmap($S, $S, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode      = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $g.InterpolationMode  = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.Clear([System.Drawing.Color]::Transparent)

    # ── Sky gradient background ───────────────────────────────────────────
    $bgRect    = [System.Drawing.Rectangle]::new(0, 0, $S, $S)
    $skyBrush  = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
                    $bgRect,
                    [System.Drawing.Color]::FromArgb(255, 95, 185, 255),
                    [System.Drawing.Color]::FromArgb(255, 30, 115, 230),
                    [System.Drawing.Drawing2D.LinearGradientMode]::Vertical)

    # Clip to rounded rectangle for sizes >= 48
    if ($S -ge 48) {
        $r    = [Math]::Max(2, [int]($S * 0.13))
        $d    = $r * 2
        $clip = New-Object System.Drawing.Drawing2D.GraphicsPath
        $clip.AddArc(0,        0,        $d, $d, 180, 90)
        $clip.AddArc($S - $d,  0,        $d, $d, 270, 90)
        $clip.AddArc($S - $d,  $S - $d,  $d, $d,   0, 90)
        $clip.AddArc(0,        $S - $d,  $d, $d,  90, 90)
        $clip.CloseFigure()
        $g.SetClip($clip)
        $clip.Dispose()
    }
    $g.FillRectangle($skyBrush, $bgRect)
    $g.ResetClip()
    $skyBrush.Dispose()

    # ── Sun (upper-right) ─────────────────────────────────────────────────
    $sunR  = [int]($S * 0.23)
    $sunCX = [int]($S * 0.69)
    $sunCY = [int]($S * 0.25)

    if ($S -ge 32) {
        $glowR  = [int]($sunR * 1.6)
        $glowBr = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(55, 255, 225, 60))
        $g.FillEllipse($glowBr, $sunCX - $glowR, $sunCY - $glowR, $glowR * 2, $glowR * 2)
        $glowBr.Dispose()
    }

    $sunBr = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 255, 210, 45))
    $g.FillEllipse($sunBr, $sunCX - $sunR, $sunCY - $sunR, $sunR * 2, $sunR * 2)
    $sunBr.Dispose()

    # ── Cloud (centre-left) ───────────────────────────────────────────────
    $cx = [int]($S * 0.08)
    $cy = [int]($S * 0.41)
    $cw = [int]($S * 0.75)
    $ch = [int]($S * 0.33)

    $shadowBr = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(55, 130, 155, 200))
    $cloudBr  = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 242, 248, 255))

    foreach ($pass in @('shadow', 'cloud')) {
        $br = if ($pass -eq 'shadow') { $shadowBr } else { $cloudBr }
        $ox = if ($pass -eq 'shadow' -and $S -ge 32) { [int]($S * 0.016) } else { 0 }
        $oy = if ($pass -eq 'shadow' -and $S -ge 32) { [int]($S * 0.022) } else { 0 }
        if ($pass -eq 'shadow' -and $S -lt 32) { continue }

        $g.FillEllipse($br, $cx+$ox + [int]($cw*0.00), $cy+$oy + [int]($ch*0.36), [int]($cw*0.40), [int]($ch*0.64))
        $g.FillEllipse($br, $cx+$ox + [int]($cw*0.24), $cy+$oy + [int]($ch*0.06), [int]($cw*0.45), [int]($ch*0.70))
        $g.FillEllipse($br, $cx+$ox + [int]($cw*0.10), $cy+$oy + [int]($ch*0.22), [int]($cw*0.57), [int]($ch*0.78))
        $g.FillRectangle($br, $cx+$ox + [int]($cw*0.05), $cy+$oy + [int]($ch*0.52), [int]($cw*0.85), [int]($ch*0.48))
    }
    $shadowBr.Dispose()
    $cloudBr.Dispose()

    # ── Raindrops (below cloud) ───────────────────────────────────────────
    $dropBr = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 60, 140, 255))
    $dw     = [Math]::Max(2, [int]($S * 0.076))
    $dh     = [Math]::Max(3, [int]($S * 0.165))
    $dy     = [int]($cy + $ch * 1.04)

    foreach ($xf in @(0.15, 0.43, 0.71)) {
        $dx   = [int]($S * $xf)
        $drop = New-Object System.Drawing.Drawing2D.GraphicsPath
        $drop.AddEllipse($dx - [int]($dw / 2), $dy, $dw, $dw)
        $pts = @(
            [System.Drawing.PointF]::new($dx - [int]($dw / 2), $dy + [int]($dw * 0.55)),
            [System.Drawing.PointF]::new($dx + [int]($dw / 2), $dy + [int]($dw * 0.55)),
            [System.Drawing.PointF]::new($dx,                  $dy + $dh)
        )
        $drop.AddPolygon($pts)
        $g.FillPath($dropBr, $drop)
        $drop.Dispose()
    }
    $dropBr.Dispose()
    $g.Dispose()

    return $bmp
}

function Save-MultiSizeIco {
    param([string]$IcoPath, [System.Drawing.Bitmap[]]$Bitmaps)

    # Encode every bitmap as PNG bytes
    $pngData = $Bitmaps | ForEach-Object {
        $ms = New-Object System.IO.MemoryStream
        $_.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
        , $ms.ToArray()      # comma forces array element, not unwrap
        $ms.Dispose()
    }

    $count  = $Bitmaps.Count
    $offset = 6 + $count * 16   # header (6) + directory entries (16 each)

    $ico = New-Object System.IO.MemoryStream
    $bw  = New-Object System.IO.BinaryWriter($ico)

    # ICO header
    $bw.Write([uint16]0)       # reserved
    $bw.Write([uint16]1)       # type = icon
    $bw.Write([uint16]$count)

    # Directory entries
    for ($i = 0; $i -lt $count; $i++) {
        $wb = if ($Bitmaps[$i].Width  -ge 256) { [byte]0 } else { [byte]$Bitmaps[$i].Width  }
        $hb = if ($Bitmaps[$i].Height -ge 256) { [byte]0 } else { [byte]$Bitmaps[$i].Height }
        $bw.Write($wb)
        $bw.Write($hb)
        $bw.Write([byte]0)       # colorCount (0 = > 256 colors)
        $bw.Write([byte]0)       # reserved
        $bw.Write([uint16]1)     # planes
        $bw.Write([uint16]32)    # bit depth
        $bw.Write([uint32]$pngData[$i].Length)
        $bw.Write([uint32]$offset)
        $offset += $pngData[$i].Length
    }

    # Image data
    foreach ($data in $pngData) { $bw.Write($data) }

    $bw.Flush()
    [System.IO.File]::WriteAllBytes($IcoPath, $ico.ToArray())
    $bw.Dispose()
    $ico.Dispose()
}

# ── Main ─────────────────────────────────────────────────────────────────────
$sizes   = @(16, 32, 48, 64, 256)
$bitmaps = $sizes | ForEach-Object { New-WeatherBitmap $_ }

$icoPath = Join-Path $PSScriptRoot "WeatherApp\WeatherApp.ico"
Save-MultiSizeIco -IcoPath $icoPath -Bitmaps $bitmaps

$bitmaps | ForEach-Object { $_.Dispose() }

Write-Host "Icon created: $icoPath" -ForegroundColor Green
