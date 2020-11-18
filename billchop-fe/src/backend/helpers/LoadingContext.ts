import { BehaviorSubject, EMPTY, interval, Observable } from "rxjs";
import { debounce } from "rxjs/operators";

class LoadingContextManager {
  private _isLoading = false;
  private isLoadingSubject = new BehaviorSubject(this._isLoading);

  public set isLoading(value: boolean) {
    this.isLoadingSubject.next(value);
  }

  public get loadingObservable(): Observable<boolean> {
    return this.isLoadingSubject.pipe(debounce((isLoading) => isLoading ? EMPTY : interval(300)));
  }
}

const LoadingContext = new LoadingContextManager();
export default LoadingContext;