import { Component, Inject, EventEmitter, Output, Input } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good } from '../../../model';
import { GoodListConfirmDeleteModalContent } from './good-list-confirmdeletemodal.component'
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'good-list-header',
    templateUrl: './good-list-header.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodListHeaderComponent {
    
    @Output() newGoodCreating: EventEmitter<Good>;
    @Output() goodsWasDeleted: EventEmitter<string[]>;
   
    @Input() categoryId: string;
    @Input() categoryName: string;
    
    private _selectedGoodIds: string[];
   
    constructor(@Inject(DataService) private dataService: DataService
            , private modalService: NgbModal) {
        this.newGoodCreating = new EventEmitter<Good>();
        this._selectedGoodIds = [];
        this.goodsWasDeleted = new EventEmitter<string[]>();
    }

    createGood(): void {        
        var good = new Good(null, 'Новый товар', 0.0, 1, null, this.categoryId);
        this.newGoodCreating.emit(good);
    }

    removeGoods(): void {
        this.openConfirmDeleteModal();
    }

    onWasGoodChecked(evt: any) {
        if(evt.checked) {
            const index = this._selectedGoodIds.indexOf(evt.goodId, 0);
            if (index == -1) {
                this._selectedGoodIds.push(evt.goodId);
            }
        }
        else {
            const index = this._selectedGoodIds.indexOf(evt.goodId, 0);
            if (index > -1) {
                this._selectedGoodIds.splice(index, 1);
            }
        }
    }

    openConfirmDeleteModal() {
        const modalRef = this.modalService.open(GoodListConfirmDeleteModalContent, { backdrop: "static" });
        modalRef.result.then((userResponse) => {
            if (userResponse == 'ok') {
                this.dataService.deleteProducts(this._selectedGoodIds)
                .subscribe((data: Good[]) => {                
                     let ids = data.map(g => g.id);
                     this.goodsWasDeleted.emit(ids);
                });    
            }
        });
    }
}

