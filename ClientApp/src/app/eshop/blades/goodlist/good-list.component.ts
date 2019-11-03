import { Component, Inject, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good, Category } from '../../../model';
import { BehaviorSubject } from 'rxjs'
import { GoodListConfirmDeleteModalContent } from './good-list-confirmdeletemodal.component'
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HeaderButton } from '../../blades/blade-header.component'
import { BladeConfig } from 'ngx-blade/esm5/ngx-blade';
import { GoodListBodyComponent } from './good-list-body.component'
import { NgxBladeComponent } from 'ngx-blade/esm5/src/app/modules/ngx-blade/ngx-blade.component';

@Component({
    selector: 'good-list-blade',
    templateUrl: './good-list.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodListComponent {

    @Output() goodWasSelected: EventEmitter<Good>;

    public bladeConfig: BladeConfig = {
        closeButton: false,
        maximizeButton: false,
        isModal: false,
    };

    private _blade: NgxBladeComponent;
    @ViewChild("goodListBlade", { static: false }) set blade(blade: NgxBladeComponent) {
        if (blade !== undefined) {
            this._blade = blade;
            this._blade.onMaximize();
        }
    }

    private bodyComponent: GoodListBodyComponent;
    @ViewChild('body', { static: false }) set _body(body: GoodListBodyComponent) {
        if (body !== undefined) {
            this.bodyComponent = body;
        }
    }

    private _categoryName: string;
    public get categoryName(): string {
        if (this.bodyComponent != null) {
            return this.bodyComponent.selectedCategoryName();
        }
        return "";
    }

    private _buttons: HeaderButton[] = [];
    public get buttons() {return this._buttons; }

    ngOnInit() {
        let newGoodButton = new HeaderButton('fa fa-plus-square fa-3x', "Новый товар", () => false);
        newGoodButton.click.subscribe(g => this.createGood());
        this._buttons.push(newGoodButton);

        let removeButton = new HeaderButton('fa fa-trash fa-3x', "Удалить", () => {
            if (this.bodyComponent == undefined)
                return true;
            return this.bodyComponent.selectedGoodIds().length == 0;
        });
        removeButton.click.subscribe(g => this.removeGoods());
        this._buttons.push(removeButton);
    }

    constructor(@Inject(DataService) private dataService: DataService
        , private modalService: NgbModal) {
        this.goodWasSelected = new EventEmitter<Good>();
        // this.newGoodCreating = new EventEmitter<Good>();
    }

    createGood(): void {
        var good = new Good(null, 'Новый товар', 0.0, 1, null, this.bodyComponent.selectedCategoryId());
        this.onGoodWasSelected(good);
    }

    removeGoods(): void {
        this.openConfirmDeleteModal();
    }

    onNewCategoryWasSelected(category: Category): void {
        this.bodyComponent.onNewCategoryWasSelected(category);
    }

    onGoodWasUpdated(updatedGood: Good): void {
        this.bodyComponent.onGoodWasUpdated(updatedGood);
    }

    onGoodWasCreated(createdGood: Good): void {
        this.bodyComponent.onGoodWasCreated(createdGood);
    }

    onGoodWasSelected(good: Good): void {
        this.goodWasSelected.emit(good);
    }

    openConfirmDeleteModal() {
        const modalRef = this.modalService.open(GoodListConfirmDeleteModalContent, { backdrop: "static" });
        modalRef.result.then((userResponse) => {
            if (userResponse == 'ok') {
                this.dataService.deleteProducts(this.bodyComponent.selectedGoodIds())
                    .subscribe((data: Good[]) => {
                        let ids = data.map(g => g.id);
                        this.bodyComponent.onGoodsWasDeleted(ids);
                    });
            }
        });
    }
}
