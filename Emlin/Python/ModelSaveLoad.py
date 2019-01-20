from sklearn.externals import joblib

def SaveModelAsJoblib(clf, filename):
	joblib.dump(clf, "../../Python/" + filename + '.joblib')

def LoadModelFromJoblib(filename):
	return joblib.load(filename)