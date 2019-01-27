from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import cross_val_score
from sklearn.svm import SVC
from sklearn.preprocessing import PolynomialFeatures

def GetSVMClassifier():
	svm_clf = Pipeline((
        ("poly_features", PolynomialFeatures(degree=3)),
		("scaler", StandardScaler()),
		("linear_svc", SVC(kernel="poly", degree=2, coef0=1, C=5)),
	))

	return svm_clf

def GetScoreFromSVM(svm_clf, all_x, all_y):
	return cross_val_score(svm_clf, all_x, all_y, cv=5, verbose=True, n_jobs=5)
