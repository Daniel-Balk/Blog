window.imageslider = {
    idCounter: 0,
    sliders: {},
    init: () => {   
        let sliders = document.getElementsByTagName("image-slider")
        for (let i = 0; i < sliders.length; i++) {
            let slider = sliders[i]
            let sliderId = imageslider.idCounter++
            slider.setAttribute("sliderId", sliderId)
            
            slider.innerHTML = `
<slider-ui>
    <div class="w-100 mt-4 mb-2" id="slider{sliderId}-full">
        <table class="w-100"> 
            <tr class="w-100">
                <td style="width: min-content">
                    <i class="fa-solid fa-chevron-left fa-xl me-2" style="cursor: pointer" onclick="imageslider.previous({sliderId})"></i>
                </td>
                <td class="w-100">
                    <img class="img-thumbnail" width="100%" style="left: 50%;position: relative;transform: translateX(-50%);" entryid="-1" src="_" id="slider{sliderId}-image"/>
                </td>
                <td style="width: min-content">
                    <i class="fa-solid fa-chevron-right fa-xl float-end ms-2" style="cursor: pointer" onclick="imageslider.next({sliderId})"></i>
                </td>
            </tr>
        </table>
    </div>
    <div class="w-100" style="scroll-behavior: smooth; overflow-x: scroll" id="slider{sliderId}-images">
        
    </div>
</slider-ui>
`.replaceAll("{sliderId}", sliderId) + slider.innerHTML;
            
            let entryview = document.getElementById("slider" + sliderId + "-images")
            let imageview = document.getElementById("slider" + sliderId + "-image")
            let entryfiles = []
            
            let entries = slider.getElementsByTagName("entry")
            
            for (let j = 0; j < entries.length; j++) {
                let entry = entries[j]
                let entryId = imageslider.idCounter++
                entry.setAttribute("entryId", entryId)
                entry.setAttribute("slider", sliderId)
                
                let fileName = entry.getAttribute("file")
                entryfiles.push({entryId: entryId, fileName: fileName})
                
                entryview.innerHTML += `<img class="img-thumbnail mb-1 me-1" style="height: 75px; user-select: text;" entryid="{entryId}" onclick="imageslider.setImage({sliderId}, {entryId})" src="{fileName}" />`.replaceAll("{fileName}",fileName).replaceAll("{sliderId}", sliderId).replaceAll("{entryId}", entryId)
            }
            
            imageslider.sliders["slider" + sliderId] = entryfiles
            imageslider.setImage(sliderId, sliderId + 1)
        }
    },
    setImage: (sliderId, entryId) => {
        let entries = imageslider.sliders["slider" + sliderId]
        let entryview = document.getElementById("slider" + sliderId + "-images")
        let imageview = document.getElementById("slider" + sliderId + "-image")
        let images = entryview.getElementsByTagName("img")

        let entry = {}
        for (let j = 0; j < entries.length; j++) {
            var elm = entries[j]
            
            if(elm.entryId === entryId)
            {
                entry = elm
                break;
            }
        }
        
        for (let i = 0; i < images.length; i++) {
            var image = images[i]
            
            if(image.getAttribute("entryid") == entryId) {
                image.style.borderColor = "gray"
                entryview.scrollLeft = image.offsetLeft
            }
            else {
                image.style.borderColor = ""
            }
        }
        
        imageview.setAttribute("entryid", entryId)
        imageview.setAttribute("src", entry.fileName)
    },
    previous: (sliderId) =>{
        let entries = imageslider.sliders["slider" + sliderId]
        let imageview = document.getElementById("slider" + sliderId + "-image")
        let currentEntry = imageview.getAttribute("entryid")

        let currentId = -1
        for (let j = 0; j < entries.length; j++) {
            var elm = entries[j]

            if(elm.entryId == currentEntry)
            {
                currentId = j
                break;
            }
        }

        let newId = currentId - 1
        if(newId == entries.length)
            newId = 0
        if(newId == -1)
            newId = entries.length - 1
        var newEntryId = entries[newId].entryId
        imageslider.setImage(sliderId, newEntryId)
    }, 
    next: (sliderId) =>{
        let entries = imageslider.sliders["slider" + sliderId]
        let imageview = document.getElementById("slider" + sliderId + "-image")
        let currentEntry = imageview.getAttribute("entryid")
        
        let currentId = -1
        for (let j = 0; j < entries.length; j++) {
            var elm = entries[j]

            if(elm.entryId == currentEntry)
            {
                currentId = j
                break;
            }
        }
        
        let newId = currentId + 1
        if(newId == entries.length)
            newId = 0
        if(newId == -1)
            newId == entries.length - 1
        var newEntryId = entries[newId].entryId
        imageslider.setImage(sliderId, newEntryId)
    }
}