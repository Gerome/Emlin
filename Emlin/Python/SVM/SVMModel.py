from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC
from sklearn.preprocessing import PolynomialFeatures

def GetSVMClassifier():
	poly_kernel_svm_clf = Pipeline((
		("scaler", StandardScaler()),
		("svm_clf", SVC(kernel="poly", degree=5, coef0=10, C=2)),
	))

	return poly_kernel_svm_clf
