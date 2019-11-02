//loader.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotifierService } from 'angular-notifier';
 

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
 
  public isLoading = new BehaviorSubject(false);
  constructor(private notifier: NotifierService) { 
  }

  showErrorNotification(message:string): void {
    this.notifier.notify("error", message );
  }
}
 