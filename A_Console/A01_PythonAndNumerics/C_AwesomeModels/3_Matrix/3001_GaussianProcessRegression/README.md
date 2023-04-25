# 💖 Gaussian Process Regression

## 🌟 Abstract

Code set for Gaussian Process Regression.






<br>

## 🌟 Figures

### 🎇 Example

before optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Example.png" width="666">

after optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Optimize.png" width="666">

```C#

// build model
var kc = new ConstantKernel(1d);
var kse = new SquaredExponentialKernel(1d);
var kwn = new WhiteNoiseKernel(1 / 15d);
var kernel = kc * kse + kwn;
var gpr = new GaussianProcessRegressionExecuter(kernel);


// (optional) optimize parameters
var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train);
PyPlotWrapper.LinePlot.DrawSimpleGraph(optimizeHistory[1]);


// fit and predict
(var Y_predict, var Y_Cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
var Y_Std = Y_Cov.Select(x => Math.Sqrt(x)).ToArray();
var Y_95CI = Y_Std.ProductForEach(1.96);
var Y_99CI = Y_Std.ProductForEach(2.58);

```







<br>

## 🌟 Memo


- ガウス回帰過程について
https://gochikika.ntt.com/Modeling/gp_regression.html


- カーネル一覧
https://observablehq.com/@herbps10/gaussian-processes


- sk-learnの使ってる組み合わせ11個
https://datachemeng.com/kernel_design_in_gpr/








<br>

## ❗Notice

Any commitment to enhance this repo will be welcomed!








<br>

## ❗Japanese 日本語

ガウス回帰過程のコード集です。

マイナーなカーネルやパラメーター最適化も実装済みです。






<br>
