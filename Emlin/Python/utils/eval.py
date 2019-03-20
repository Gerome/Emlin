from matplotlib import pyplot as plt
from sklearn.model_selection import StratifiedKFold, cross_val_score


def show_precision_recall(fpr, tpr, roc_auc):
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


def get_score(clf, all_x, all_y):

    for n in range(5):
        strat_k_fold = StratifiedKFold(n_splits=10, shuffle=True, random_state=n)
        return cross_val_score(clf, all_x, all_y, cv=strat_k_fold, verbose=True, n_jobs=5)