import { Component, ViewChild, ChangeDetectorRef, Inject } from '@angular/core';
import { DataService } from './services/data.service'
import { GoodListComponent } from './eshop/blades/goodlist/good-list.component'
import { EditGoodComponent } from './eshop/blades/goodedit/good-edit.component'
import { BladeConfig } from 'ngx-blade/esm5/ngx-blade';
import { NgxBladeComponent } from 'ngx-blade/esm5/src/app/modules/ngx-blade/ngx-blade.component';
import { Category, Good } from './model'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DataService]
})
export class AppComponent {
  title = 'Mini eshop App';
 
  constructor(private changeDetector: ChangeDetectorRef, @Inject(DataService) private dataService: DataService) {
  }

  private isVisibleGoodListBlade: boolean = false;

  private goodListComponent: GoodListComponent;
  @ViewChild('goodlist', { static: false }) set goodListEl(goodListEl: GoodListComponent) {
    if (goodListEl !== undefined) {
      this.goodListComponent = goodListEl;
    }
  }

  private isVisibleEditGoodBlade: boolean = false;

  private editGoodComponent: EditGoodComponent;
  @ViewChild('goodedit', { static: false }) set editGoodEl(editGoodEl: EditGoodComponent) {
    if (editGoodEl !== undefined) {
      this.editGoodComponent = editGoodEl;
    }
  }

  onNewCategoryWasSelected(category: Category): void {
    if (this.isVisibleGoodListBlade === false) {
      this.isVisibleGoodListBlade = true;
      this.changeDetector.detectChanges();
    }

    if (this.goodListComponent != null) {
      this.goodListComponent.onNewCategoryWasSelected(category);
      // close blade because tha blade is displayed good on unselected category
      if(this.editGoodComponent != undefined) {
        this.editGoodComponent.close();
        this.isVisibleEditGoodBlade = false;
        this.changeDetector.detectChanges();
      }
    }
  }

  onGoodWasSelected(good: Good): void {
    if (this.isVisibleEditGoodBlade === false) {
      this.isVisibleEditGoodBlade = true;
      this.changeDetector.detectChanges();
    }
    if (this.editGoodComponent != null) {
      this.editGoodComponent.newGoodWasSelected(good);
    }
  }
}
