import { Component, Inject, EventEmitter, Output, Input } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good, Category } from '../../../model';
import { createImagePath } from '../../../common'
import { BehaviorSubject } from 'rxjs'

@Component({
    selector: 'good-list-body',
    templateUrl: './good-list-body.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodListBodyComponent {

    public goodList: Good[]; // массив товаров

    public selectedGoodIndex: number;   
    private _selectedGoodName: string;

    public PAGE_SIZE: number = 10;
    public activePage: number = 1;
    private _goodSize: number;
    public get goodSize() {return this._goodSize;}

    private _selectedCategoryId: string;
    private _selectedCategoryName: string;

    public selectedCategoryId(): string {
        return this._selectedCategoryId;
    }
    
    public selectedCategoryName(): string {
        return this._selectedCategoryName;
    }

    private _selectedGoodIds: string[];
    public selectedGoodIds(): string[] {
        return this._selectedGoodIds;
    }

    @Output() goodWasSelected: EventEmitter<Good>;
 
    constructor(@Inject(DataService) private dataService: DataService) {
        this.selectedGoodIndex = -1;
        this.goodList = []
        this._goodSize = 0;
        this._selectedGoodIds = [];
        this.goodWasSelected = new EventEmitter<Good>();
    }

    onNewCategoryWasSelected(category: Category): void {
        this.getGoods(category.id, category.name);
    }

    onPageChange(page: number) {
        this.dataService.getGoods(this._selectedCategoryId, (page - 1) * this.PAGE_SIZE, this.PAGE_SIZE)
            .subscribe((data: Good[]) => {
                this.selectedGoodIndex = -1;
                this.goodList = data;
                this.activePage = page;
            })
    }

    onGoodWasSelected(event: any, index: number): void {
        if (index < this.goodList.length) {
            this.selectedGoodIndex = index;
            this.goodWasSelected.emit(this.goodList[index]);
            this._selectedGoodName = this.goodList[index].name;
        }
    }

    onGoodWasUpdated(updatedGood: Good): void {
        var foundIndex = this.goodList.findIndex(x => x.id == updatedGood.id);
        this.goodList[foundIndex] = updatedGood;
        this._selectedGoodName = this.goodList[this.selectedGoodIndex].name;
    }

    onGoodWasCreated(createdGood: Good): void {
        this.getGoods(this._selectedCategoryId, this._selectedCategoryName);
    }

    onGoodsWasDeleted(deletedGoodIds: string[]): void {
        this.getGoods(this._selectedCategoryId, this._selectedCategoryName);

        deletedGoodIds.forEach(g => {
            const good = this.goodList.find(k => k.id == g);
            var idx = this.goodList.indexOf(good);
            if (idx != -1) {
                --this._goodSize;
                this.goodList.splice(idx, 1); // The second parameter is the number of elements to remove.
            }

            idx = this._selectedGoodIds.indexOf(g);
            if (idx != -1) {
                this._selectedGoodIds.splice(idx, 1);
            }
        });
    }

    private getGoods(categoryId: string, categoryName: string): void {
        this.dataService.getGoods(categoryId, (this.activePage - 1) * this.PAGE_SIZE, this.PAGE_SIZE)
            .subscribe((data: Good[]) => {
                this.selectedGoodIndex = -1;
                this.goodList = data;
                this.activePage = 1;
                this._selectedCategoryId = categoryId;
                this._selectedCategoryName = categoryName;
            })

        this.dataService.getGoodsCount(categoryId)
            .subscribe((size: number) => {
                this._goodSize = size;
            });
    }

    createImgPath = (serverPath: string) => {
        return createImagePath(serverPath);
    }

    toogleGoodSelection(evt: any, goodId: string): void {
        if (evt.target.checked) {
            const index = this._selectedGoodIds.indexOf(goodId, 0);
            if (index == -1) {
                this._selectedGoodIds.push(goodId);
            }
        }
        else {
            const index = this._selectedGoodIds.indexOf(goodId, 0);
            if (index > -1) {
                this._selectedGoodIds.splice(index, 1);
            }
        }
    }
}

