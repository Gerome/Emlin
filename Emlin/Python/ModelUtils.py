from sklearn.externals import joblib
from sklearn.model_selection import cross_val_score
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import os
from sklearn.metrics import precision_recall_curve
from sklearn.utils.fixes import signature
from sklearn.model_selection import StratifiedKFold

def SaveModelAsJoblib(clf, directory, filename):
    joblib.dump(clf, os.path.join(directory, filename + '.joblib'))


def LoadModelFromJoblib(directory, filename):
    return joblib.load(os.path.join(directory, filename))


def plot_confusion_matrix(df_confusion, title='Confusion matrix', cmap=plt.cm.gray_r):
    plt.matshow(df_confusion, cmap=cmap) # imshow
    #plt.title(title)
    plt.colorbar()
    tick_marks = np.arange(len(df_confusion.columns))
    plt.xticks(tick_marks, df_confusion.columns, rotation=45)
    plt.yticks(tick_marks, df_confusion.index)
    #plt.tight_layout()
    plt.ylabel(df_confusion.index.name)
    plt.xlabel(df_confusion.columns.name)
    plt.show()



def ShowConfusionMatrix(clf, test_x, test_y):
    y_actu = pd.Series(test_y, name='Actual')
    y_pred = pd.Series(clf.predict(test_x), name='Predicted')
    df_confusion = pd.crosstab(y_actu, y_pred)
    print(df_confusion)
    plot_confusion_matrix(df_confusion)


def ShowPrecisionRecall(fpr, tpr, roc_auc):
    plt.title('Receiver Operating Characteristic')
    plt.plot(fpr, tpr, 'b', label='AUC = %0.2f' % roc_auc)
    plt.legend(loc='lower right')
    plt.plot([0, 1], [0, 1], 'r--')
    plt.xlim([0, 1])
    plt.ylim([0, 1])
    plt.ylabel('True Positive Rate')
    plt.xlabel('False Positive Rate')
    plt.title('ROC Curve of kNN')
    plt.show()


def GetScoreFromCLF(clf, all_x, all_y):

    for n in range(5):
        strat_k_fold = StratifiedKFold(n_splits=10, shuffle=True, random_state=n)
        return cross_val_score(clf, all_x, all_y, cv=strat_k_fold, verbose=True, n_jobs=5)
