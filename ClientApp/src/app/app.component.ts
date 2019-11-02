import { Component, ViewChild, AfterViewInit, ChangeDetectorRef, OnInit, Inject } from '@angular/core';
import { DataService } from './services/data.service'
import { GoodListComponent } from './eshop/blades/goodlist/good-list.component'
import { EditGoodHeaderComponent } from './eshop/blades/goodedit/good-edit-header.component'
import { BladeConfig } from 'ngx-blade/esm5/ngx-blade';
import { NgxBladeComponent } from 'ngx-blade/esm5/src/app/modules/ngx-blade/ngx-blade.component';
import { Category, Good } from './model'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DataService]
})
export class AppComponent implements AfterViewInit, OnInit {
  title = 'Mini eshop App';

  constructor(private changeDetector: ChangeDetectorRef, @Inject(DataService) private dataService: DataService) {
  }

  public bladeConfig: BladeConfig = {
    closeButton: false,
    maximizeButton: false,
    isModal: false,
  };

  @ViewChild("categoryBlade", { static: false })
  private categoryBlade: NgxBladeComponent;

  private goodListBlade: NgxBladeComponent;
  @ViewChild("goodListBlade", { static: false }) set content(content: NgxBladeComponent) {
    if (content !== undefined) {
      this.goodListBlade = content;
      this.goodListBlade.onMaximize();
    }
  }
  private isVisibleGoodListBlade: boolean = false;

  private goodListComponent: GoodListComponent = new GoodListComponent(this.dataService);
  @ViewChild('goodlist', { static: false }) set goodListEl(goodListEl: GoodListComponent) {
    if (goodListEl !== undefined) {
      this.goodListComponent = goodListEl;
    }
  }

  private editGoodBlade: NgxBladeComponent;
  @ViewChild("editGoodBlade", { static: false }) set editGood(editGood: NgxBladeComponent) {
    if (editGood!== undefined) {
      this.editGoodBlade = editGood;
      this.editGoodBlade.onMaximize();
    }
  }
  private isVisibleEditGoodBlade: boolean = false;

  private editHeaderGoodComponent: EditGoodHeaderComponent;
  @ViewChild('goodeditheader', { static: false }) set editGoodHeaderEl(editGoodHeaderEl: EditGoodHeaderComponent) {
    if (editGoodHeaderEl !== undefined) {
      this.editHeaderGoodComponent = editGoodHeaderEl;
    }
  }

  ngAfterViewInit() {
    this.categoryBlade.onMaximize();
  }

  ngOnInit(){
    this.dataService.getCategories();
  }

  onNewCategoryWasSelected(category: Category): void {
    if (this.isVisibleGoodListBlade === false) {
      this.isVisibleGoodListBlade = true;
      this.changeDetector.detectChanges();
    }

    if (this.goodListComponent != null) {
      this.goodListComponent.onNewCategoryWasSelected(category);
      // close blade because tha blade is displayed good on unselected category
      if(this.editGoodBlade != undefined) {
        this.editGoodBlade.close();
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
    if (this.editHeaderGoodComponent != null) {
      this.editHeaderGoodComponent.newGoodWasSelected(good);
    }
  }
}
